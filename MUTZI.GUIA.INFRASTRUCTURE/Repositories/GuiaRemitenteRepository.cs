using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemitenteRepository : IGuiaRemitenteRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemitenteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<(List<GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request, bool export = false)
        {
            try
            {
                var query = _context.GuiaRemisions
                    .Include(m => m.Remitente)
                        .ThenInclude(m => m.RucNavigation)
                    .Include(m => m.Destinatario)
                        .ThenInclude(m => m.RucNavigation)
                    .AsQueryable();
                if (request.Columns != null && request.Columns.Any())
                {
                    foreach (var col in request.Columns)
                    {
                        var val = col.Search.Value?.ToLower();
                        var key = col.Data?.ToLower();
                        if (string.IsNullOrEmpty(val)) continue;
                        if (key.Contains("fecha"))
                        {
                            var parts = val.Split('|', StringSplitOptions.RemoveEmptyEntries);

                            if (parts.Length == 1)
                            {
                                if (DateTime.TryParse(parts[0], out var fechaDt))
                                {
                                    var fecha = DateOnly.FromDateTime(fechaDt);
                                    query = key switch
                                    {
                                        "fecha_registro" => query.Where(x => DateOnly.FromDateTime(x.FechaCreacion.Value) == fecha),
                                        "fecha_emision" => query.Where(x => x.Fecha == fecha),
                                        _ => query
                                    };
                                }
                            }
                            else
                            {
                                if (DateTime.TryParse(parts[0], out var iniDt) && DateTime.TryParse(parts[1], out var finDt))
                                {
                                    var fechaIni = DateOnly.FromDateTime(iniDt);
                                    var fechaFin = DateOnly.FromDateTime(finDt);
                                    query = key switch
                                    {
                                        "fecha_registro" => query.Where(x => DateOnly.FromDateTime(x.FechaCreacion.Value) >= fechaIni
                                                                          && DateOnly.FromDateTime(x.FechaCreacion.Value) <= fechaFin),
                                        "fecha_emision" => query.Where(x => x.Fecha >= fechaIni && x.Fecha <= fechaFin),
                                        _ => query
                                    };
                                }
                            }
                            continue;
                        }
                        if (key.Contains("id_remitente"))
                        {
                            var idsStrings = val.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            if (idsStrings.Length == 1)
                            {
                                if (int.TryParse(idsStrings[0], out int idUnico))
                                    query = query.Where(x => x.RemitenteId == idUnico);
                            }
                            else
                            {
                                var idsInt = idsStrings.Select(int.Parse).ToList();
                                query = query.Where(x => idsInt.Contains(x.RemitenteId));
                            }
                            continue;
                        }
                        if (key.Contains("tipo_traslado"))
                        {
                            var tipoStrings = val.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            if (tipoStrings.Length == 1)
                            {
                                query = query.Where(x => x.TipoTraslado.ToLower() == tipoStrings[0]);
                            }
                            else
                            {
                                query = query.Where(x => tipoStrings.Contains(x.TipoTraslado.ToLower()));
                            }
                            continue;
                        }
                        if (key.Contains("estado"))
                        {
                            var estadoStrings = val.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            if (estadoStrings.Length == 1)
                            {
                                query = query.Where(x => x.Estado.ToLower() == estadoStrings[0]);
                            }
                            else
                            {
                                query = query.Where(x => estadoStrings.Contains(x.Estado.ToLower()));
                            }
                            continue;
                        }
                        switch (key)
                        {
                            case "ruc": query = query.Where(x => x.Ruc == val); break;
                            case "tipo_guia": query = query.Where(x => x.TipoGuia.ToLower().Contains(val)); break;
                            case "correlativo": query = query.Where(x => x.Correlativo.ToLower() == val); break;
                            case "serie_guia": query = query.Where(x => x.Serie.ToLower() == val); break;
                            //case "tipo_traslado": query = query.Where(x => x.TipoTraslado == val); break;
                            case "tipo_transporte": query = query.Where(x => x.TipoTransporte.ToLower() == val); break;
                            case "id_destinatario":
                                if (int.TryParse(val, out int idD))
                                    query = query.Where(x => x.DestinatarioId == idD);
                                break;
                            case "search":
                                query = query.Where(x => x.Ruc.Contains(val) ||
                                    x.RucNavigation.Empresa.ToLower().Contains(val) || x.Ruc.Contains(val) ||
                                    x.Remitente.Descripcion.ToLower().Contains(val) || x.TipoGuia.ToLower().Contains(val) ||
                                    x.NumeroGuia.Contains(val) || x.TipoTraslado.ToLower().Contains(val) ||
                                    x.TipoTransporte.ToLower().Contains(val) || //x.Hora.Contains(val)||
                                    x.Destinatario.RucNavigation.Empresa.ToLower().Contains(val) || x.Destinatario.Ruc.Contains(val) ||
                                    x.Origens.FirstOrDefault().Ubigeo.Distrito.ToLower().Contains(val) ||
                                    x.Destinos.FirstOrDefault().Ubigeo.Distrito.ToLower().Contains(val) ||
                                    x.EmpleadoNombreCreacion.ToLower().Contains(val) || x.EmpleadoNombreEdicion.ToLower().Contains(val) ||
                                    x.Estado.Contains(val)
                                );
                                break;
                        }
                    }
                }

                query = query.OrderByDescending(x => x.GuiaId);

                var totalRecords = await query.CountAsync();
                if (!export)
                {
                    query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
                }
                var data = await query
                    .Select(g => new GuiaRemision
                    {
                        GuiaId = g.GuiaId,
                        TipoGuia = g.TipoGuia,
                        Ruc = g.Ruc,
                        RucNavigation = new VentaConfig
                        {
                            EmailPrincipal = g.RucNavigation.EmailPrincipal,
                            Empresa = g.RucNavigation.Empresa
                        },
                        TipoTransporte = g.TipoTransporte,
                        TipoTraslado = g.TipoTraslado,
                        NumeroGuia = g.NumeroGuia,
                        Fecha = g.Fecha,
                        Hora = g.Hora,
                        RespuestaTicket = g.RespuestaTicket,
                        Remitente = new Establecimiento {
                            Ruc = g.Remitente.Ruc,
                            Descripcion = g.Remitente.Descripcion,
                            Direccion = g.Remitente.Direccion,
                            RucNavigation = new VentaConfig
                            {
                                Empresa = g.Remitente.RucNavigation.Empresa
                            }
                        },
                        Destinatario = new Establecimiento
                        {
                            Ruc = g.Destinatario.Ruc,
                            Descripcion = g.Destinatario.Descripcion,
                            Direccion = g.Destinatario.Direccion,
                            RucNavigation = new VentaConfig
                            {
                                Empresa = g.Destinatario.RucNavigation.Empresa
                            }
                        },
                        Proveedor = g.Proveedor == null ? null : new Proveedor
                        {
                            TipoDocumento = g.Proveedor.TipoDocumento,
                            NumeroDocumento = g.Proveedor.NumeroDocumento,
                            RazonSocial = g.Proveedor.RazonSocial
                        },
                        Origens = g.Origens.Select(o => new Origen { Ubigeo = o.Ubigeo }).ToList(),
                        Destinos = g.Destinos.Select(d => new Destino { Ubigeo = d.Ubigeo }).ToList(),
                        FechaCreacion = g.FechaCreacion,
                        EmpleadoNombreCreacion = g.EmpleadoNombreCreacion,
                        FechaEdicion = g.FechaEdicion,
                        EmpleadoNombreEdicion = g.EmpleadoNombreEdicion,
                        Estado = g.Estado,
                        IdEstado = g.IdEstado,
                        EstadoSunat = g.EstadoSunat,
                        IdEstadoSunat = g.IdEstadoSunat
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return (data, totalRecords);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<GuiaRemision>> GetAllAsync()
        {
            return await _context.GuiaRemisions
                .Include(r => r.Remitente)
                    .ThenInclude(rem => rem.Ubigeo)
                .Include(r => r.Remitente)
                    .ThenInclude(rem => rem.RucNavigation)
                .Include(d => d.Destinatario)
                    .ThenInclude(des => des.Ubigeo)
                .Include(p => p.Proveedor)
                    .ThenInclude(pro => pro.Ubigeo)
                .Include(dest => dest.Destinos)
                    .ThenInclude(de => de.Ubigeo)
                .Include(o => o.Origens)
                    .ThenInclude(or => or.Ubigeo)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.ConductorGuids)
                        .ThenInclude(con => con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.TransporteGuids)
                        .ThenInclude(tr => tr.Transporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.CodigoUmNavigation)
                .Include(doc => doc.DocRelacionados)
                //.Include(e=>e.EmailDestinatarioGuids)
                .ToListAsync();
        }
        public async Task<GuiaRemision> GetByTicketAsync(string ticket)
        {
            return await _context.GuiaRemisions.Include(r => r.Remitente)
                .FirstOrDefaultAsync(g => g.RespuestaTicket == ticket);
        }
        public async Task<GuiaRemision> GetByIdAsync(int id)
        {
            return await _context.GuiaRemisions
                .Where(g => g.GuiaId == id)
                .Select(g => new GuiaRemision
                {
                    GuiaId = g.GuiaId,
                    TipoGuia = g.TipoGuia,
                    Ruc = g.Ruc,
                    RucNavigation = new VentaConfig
                    {
                        EmailPrincipal = g.RucNavigation.EmailPrincipal
                    },
                    TipoTransporte = g.TipoTransporte,
                    TipoTraslado = g.TipoTraslado,
                    NumeroGuia = g.NumeroGuia,
                    Fecha = g.Fecha,
                    Hora = g.Hora,
                    Observacion = g.Observacion,
                    Remitente = new Establecimiento
                    {
                        EstablecimientoId = g.Remitente.EstablecimientoId,
                        Ruc = g.Remitente.Ruc!,
                        Descripcion = g.Remitente.Descripcion,
                        UbigeoId = g.Remitente.UbigeoId,
                        Ubigeo = g.Remitente.Ubigeo,
                        Direccion = g.Remitente.Direccion,
                        Email = g.Remitente.Email,
                        Pais = g.Remitente.Pais,
                        Serie = g.Remitente.Serie,
                        CodigoSunat = g.Remitente.CodigoSunat,
                        FechaRegistro = g.Remitente.FechaRegistro,
                        FechaModifico = g.Remitente.FechaModifico,
                        UsuarioRegistroNombre = g.Remitente.UsuarioRegistroNombre,
                        UsuarioModificoNombre = g.Remitente.UsuarioModificoNombre,
                        Estado = g.Remitente.Estado,
                        IdEstado = g.Remitente.IdEstado
                    },
                    RemiteTransportista = g.RemiteTransportista == null ? null : new RemiteTransportistum
                    {
                        RemiteTransportistaId = g.RemiteTransportista.RemiteTransportistaId,
                        TipoDocumento = g.RemiteTransportista.TipoDocumento,
                        NumeroDocumento = g.RemiteTransportista.NumeroDocumento,
                        RazonSocial = g.RemiteTransportista.RazonSocial,
                        UbigeoId = g.RemiteTransportista.UbigeoId,
                        Ubigeo= g.RemiteTransportista.Ubigeo,
                        Direccion = g.RemiteTransportista.Direccion,
                        Pais = g.RemiteTransportista.Pais,
                        CodigoSunat = g.RemiteTransportista.CodigoSunat,
                        FechaCreacion = g.RemiteTransportista.FechaCreacion,
                        EmpleadoNombreCreacion = g.RemiteTransportista.EmpleadoNombreCreacion,
                        FechaEdicion = g.RemiteTransportista.FechaEdicion,
                        EmpleadoNombreEdicion = g.RemiteTransportista.EmpleadoNombreEdicion,
                        EmailRemiteTransportistaGuids = g.EmailRemiteTransportistaGuids,
                        Estado = g.RemiteTransportista.Estado,
                        IdEstado = g.RemiteTransportista.IdEstado
                    },
                    Destinatario = new Establecimiento
                    {
                        EstablecimientoId = g.Destinatario.EstablecimientoId,
                        Ruc = g.Destinatario.Ruc,
                        UbigeoId = g.Destinatario.UbigeoId,
                        Ubigeo = g.Destinatario.Ubigeo,
                        Direccion = g.Destinatario.Direccion,
                        Pais = g.Destinatario.Pais,
                        CodigoSunat = g.Destinatario.CodigoSunat,
                        FechaRegistro = g.Destinatario.FechaRegistro,
                        UsuarioRegistro = g.Destinatario.UsuarioRegistro,
                        FechaModifico = g.Destinatario.FechaModifico,
                        UsuarioModifico = g.Destinatario.UsuarioModifico,
                        Estado = g.Destinatario.Estado,
                        IdEstado = g.Destinatario.IdEstado
                    },
                    Proveedor = g.Proveedor == null ? null : new Proveedor
                    {
                        ProveedorId = g.Proveedor.ProveedorId,
                        TipoDocumento = g.Proveedor.TipoDocumento,
                        NumeroDocumento = g.Proveedor.NumeroDocumento,
                        RazonSocial = g.Proveedor.RazonSocial,
                        Ubigeo = g.Proveedor.Ubigeo,
                        Direccion = g.Proveedor.Direccion,
                        Email = g.Proveedor.Email,
                        Pais = g.Proveedor.Pais,
                        CodigoSunat = g.Proveedor.CodigoSunat,
                        FechaRegistro = g.Proveedor.FechaRegistro,
                        UsuarioRegistro = g.Proveedor.UsuarioRegistro,
                        FechaModifico = g.Proveedor.FechaModifico,
                        UsuarioModifico = g.Proveedor.UsuarioModifico,
                        Estado = g.Proveedor.Estado,
                        IdEstado = g.Proveedor.IdEstado
                    },
                    GuiaDetalles = g.GuiaDetalles.Select(det => new GuiaDetalle
                    {
                        GuiaDetalleId = det.GuiaDetalleId,
                        Codigo = det.Codigo,
                        Descripcion = det.Descripcion,
                        Cantidad = det.Cantidad,
                        CodigoUm = det.CodigoUm,
                        CodigoUmNavigation = new UnidadMedidum
                        {
                            CodigoUm = det.CodigoUmNavigation.CodigoUm,
                            Descripcion = det.CodigoUmNavigation.Descripcion
                        }
                    }).ToList(),

                    // Datos de Envío y Conductores
                    DatosEnvios = g.DatosEnvios.Select(env => new DatosEnvio
                    {
                        DatosEnvioId = env.DatosEnvioId,
                        MotivoEnvio = env.MotivoEnvio,
                        FechaEnvio = env.FechaEnvio,
                        PesoBruto = env.PesoBruto,
                        CodigoUm = env.CodigoUm,
                        RucEmpresaCurrier = env.RucEmpresaCurrier,
                        RazonSocialCurrier = env.RazonSocialCurrier,
                        RegistroMtcCurrier = env.RegistroMtcCurrier,
                        IndicadorVehiculoConductor = env.IndicadorVehiculoConductor,
                        ConductorGuids = env.ConductorGuids.Select(cg => new ConductorGuid
                        {
                            ConductorGuidId = cg.ConductorGuidId,
                            DatosEnvioId = cg.DatosEnvioId,
                            ConductorId = cg.ConductorId,
                            Conductor = new Conductor
                            {
                                ConductorId = cg.Conductor.ConductorId,
                                TipoDocumento = cg.Conductor.TipoDocumento,
                                NumeroDocumento = cg.Conductor.NumeroDocumento,
                                Nombres = cg.Conductor.Nombres,
                                Apellidos = cg.Conductor.Apellidos,
                                Cargo = cg.Conductor.Cargo,
                                Licencia = cg.Conductor.Licencia
                            }
                        }).ToList(),
                        TransporteGuids = env.TransporteGuids.Select(tg => new UnidadTransporteGuid
                        {
                            TransporteGuidId = tg.TransporteGuidId,
                            DatosEnvioId = tg.DatosEnvioId,
                            TransporteId = tg.TransporteId,
                            Transporte = new UnidadTransporte
                            {
                                UnidadTransporteId = tg.Transporte.UnidadTransporteId,
                                Placa = tg.Transporte.Placa,
                                Tarjeta = tg.Transporte.Tarjeta,
                                NroAutorizacion = tg.Transporte.NroAutorizacion,
                                EmisorVehicular = tg.Transporte.EmisorVehicular,
                                CodEmisorVehicular = tg.Transporte.CodEmisorVehicular,
                            }
                        }).ToList(),
                    }).ToList(),
                    Origens = g.Origens.Select(o => new Origen
                    {
                        OrigenId = o.OrigenId,
                        UbigeoId = o.UbigeoId,
                        Direccion = o.Direccion,
                        Ubigeo = o.Ubigeo,
                        Pais = o.Pais
                    }).ToList(),
                    Destinos = g.Destinos.Select(d => new Destino
                    {
                        DestinoId = d.DestinoId,
                        UbigeoId = d.UbigeoId,
                        Direccion = d.Direccion,
                        Ubigeo = d.Ubigeo,
                        Pais = d.Pais
                    }).ToList(),
                    DocRelacionados = g.DocRelacionados.ToList(),
                    FechaCreacion = g.FechaCreacion,
                    EmpleadoNombreCreacion = g.EmpleadoNombreCreacion,
                    Estado = g.Estado,
                    IdEstado = g.IdEstado
                })
                .FirstOrDefaultAsync();
        }
        public async Task<GuiaRemision> GetByIdAsync2(int id)
        {
            return await _context.GuiaRemisions
                .Include(r => r.Remitente)
                    .ThenInclude(rem => rem.Ubigeo)
                .Include(rt => rt.RemiteTransportista)
                    .ThenInclude(rtu => rtu.Ubigeo)
                .Include(d => d.Destinatario)
                    .ThenInclude(des => des.Ubigeo)
                .Include(p => p.Proveedor)
                    .ThenInclude(pro => pro.Ubigeo)
                .Include(dest => dest.Destinos)
                    .ThenInclude(de => de.Ubigeo)
                .Include(o => o.Origens)
                    .ThenInclude(or => or.Ubigeo)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.ConductorGuids)
                        .ThenInclude(con => con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.TransporteGuids)
                        .ThenInclude(tr => tr.Transporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.CodigoUmNavigation)
                .Include(doc => doc.DocRelacionados)
                .Include(e => e.EmailDestinatarioGuids)
                .Include(erm => erm.EmailRemiteTransportistaGuids)
                .FirstOrDefaultAsync(g => g.GuiaId == id);
        }

        public async Task<(int GuiaId, string NumeroGuia)> CreateGuiaRemisionTransactionalAsync(
            Establecimiento remitente,
            Establecimiento destinatario,
            Proveedor proveedor,
            List<ConductorGuid> conductor,
            List<UnidadTransporteGuid> transporte,
            GuiaRemision guiaRemision,
            DatosEnvio datosEnvio,
            Origen origen,
            List<Destino> destino,
            List<GuiaDetalle> detalles,
            List<DocRelacionado> docRelacionados,
            List<EmailDestinatarioGuid> emailDestinatarioGuia
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Guía
                guiaRemision.RemitenteId = remitente.EstablecimientoId;
                guiaRemision.Ruc = remitente.Ruc;
                guiaRemision.DestinatarioId = destinatario.EstablecimientoId;
                guiaRemision.Estado = "REGISTRADO";
                guiaRemision.IdEstado = 0;
                //if (guiaRemision.TipoTraslado == "COMPRA") { guiaRemision.ProveedorId = proveedor.ProveedorId; }

                //string serie = guiaRemision.Serie ?? "T001";
                string serie = "T" + remitente.Serie.Trim();

                //Buscar el último correlativo para esa serie
                var lastGuia = await _context.GuiaRemisions
                    .Where(g => g.Serie == serie)
                    .OrderByDescending(g => g.Correlativo)
                    .FirstOrDefaultAsync();

                int nextCorrelativo = 1;
                if (lastGuia != null && int.TryParse(lastGuia.Correlativo, out int lastCorrelativo))
                {
                    nextCorrelativo = lastCorrelativo + 1;
                }

                //Formatear el correlativo a 8 dígitos
                string correlativoStr = nextCorrelativo.ToString("D8");

                //Guardar los campos
                guiaRemision.Correlativo = correlativoStr;
                guiaRemision.Serie = serie;
                guiaRemision.NumeroGuia = $"{serie}-{correlativoStr}";


                guiaRemision.TipoGuia = "REMITENTE";
                guiaRemision.RespuestaTicket = "REGISTRADO";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;
                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // ConductorgUID
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    var conductorGuia = new ConductorGuid
                    {
                        ConductorId = condu.ConductorId,
                        DatosEnvioId = datosEnvio.DatosEnvioId
                    };
                    await _context.ConductorGuids.AddAsync(conductorGuia);
                    await _context.SaveChangesAsync();
                }

                // TransportegUID
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    var transporteGuia = new UnidadTransporteGuid
                    {
                        TransporteId = tr.TransporteId,
                        DatosEnvioId = datosEnvio.DatosEnvioId
                    };
                    await _context.UnidadTransporteGuids.AddAsync(transporteGuia);
                    await _context.SaveChangesAsync();
                }

                // DocRelacionados
                if (docRelacionados is not null)
                {
                    foreach (var doc in docRelacionados)
                    {
                        doc.GuiaId = guiaRemision.GuiaId;
                        await _context.DocRelacionados.AddAsync(doc);
                    }
                    await _context.SaveChangesAsync();
                }

                //EmailDestinatarioGuia
                if (emailDestinatarioGuia is not null)
                {
                    foreach (var email in emailDestinatarioGuia)
                    {
                        email.GuiaId = guiaRemision.GuiaId;
                        email.DestinatarioId = destinatario.EstablecimientoId;
                        await _context.EmailDestinatarioGuids.AddAsync(email);
                    }
                    await _context.SaveChangesAsync();
                }

                // Proveedor
                if(guiaRemision.TipoTraslado == "COMPRA" || guiaRemision.TipoTraslado == "RECOJO_BIENES_TRANSFORMADOS"  || guiaRemision.TipoTraslado == "OTROS")
                {
                    guiaRemision.ProveedorId = proveedor.ProveedorId;
                }

                //Origen
                origen.GuiaId = guiaRemision.GuiaId;
                await _context.Origens.AddAsync(origen);
                await _context.SaveChangesAsync();

                foreach (var dest in destino)
                {
                    dest.GuiaId = guiaRemision.GuiaId;
                    await _context.Destinos.AddAsync(dest);
                }
                await _context.SaveChangesAsync();

                // Detalles
                foreach (var detalle in detalles)
                {
                    detalle.GuiaId = guiaRemision.GuiaId;
                    await _context.GuiaDetalles.AddAsync(detalle);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                //retornar el ID de la guía creada y asimismo el número de guía.
                return (guiaRemision.GuiaId, guiaRemision.NumeroGuia);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estadoSunat, int idEstadoSunat)
        {
            var guia = await _context.GuiaRemisions.FindAsync(guiaId);
            if (guia == null)
                throw new KeyNotFoundException("Guía no encontrada");

            guia.RespuestaTicket = ticket;
            guia.EstadoSunat = estadoSunat;
            guia.IdEstadoSunat = (sbyte)idEstadoSunat;
            guia.Estado = estadoSunat;
            guia.IdEstado = (sbyte)idEstadoSunat;
            guia.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        //Actualizar guía de remisión remitente
        public async Task UpdateAsync(GuiaRemision guiaRemision)
        {
            if (guiaRemision == null)
            {
                throw new ArgumentNullException(nameof(guiaRemision), "La guía de remisión remitente no puede ser nula");
            }
            _context.GuiaRemisions.Update(guiaRemision);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var guiaRemision = await _context.GuiaRemisions.FindAsync(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("Guia de remisión no encontrada");
            }
            _context.GuiaRemisions.Remove(guiaRemision);
            await _context.SaveChangesAsync();
        }

        //Proveedor
        public async Task<Proveedor> GetProveedorByRucAsync(string ruc)
        {
            return await _context.Proveedors.FirstOrDefaultAsync(p => p.NumeroDocumento == ruc);
        }
        public async Task<Proveedor> CreateProveedorAsync(Proveedor proveedor)
        {
            try
            {
                if (proveedor == null)
                {
                    throw new ArgumentNullException(nameof(proveedor), "El objeto proveedor no puede ser nulo");
                }
                await _context.Proveedors.AddAsync(proveedor);
                await _context.SaveChangesAsync();
                return proveedor;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        //Conductor
        public async Task<Conductor> GetConductorByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
        }
        public async Task<Conductor> CreateConductorAsync(Conductor conductor)
        {
            try
            {
                if (conductor == null)
                {
                    throw new ArgumentNullException(nameof(conductor), "El objeto conductor no puede ser nulo");
                }
                await _context.Conductors.AddAsync(conductor);
                await _context.SaveChangesAsync();
                return conductor;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                else if (msg.Contains("licencia_UNIQUE")) { msg = "Nro Licencia ya se encuentra registrado"; }
                throw new Exception(msg);
            }
        }
        //Transporte
        public async Task<UnidadTransporte> GetTransporteByPlacaAsync(string placa)
        {
            return await _context.UnidadTransportes.FirstOrDefaultAsync(t => t.Placa == placa);
        }
        public async Task<UnidadTransporte> CreateTransporteAsync(UnidadTransporte transporte)
        {
            try
            {
                if (transporte == null)
                {
                    throw new ArgumentException(nameof(transporte), "El objeto transporte no puede ser nulo");
                }
                await _context.UnidadTransportes.AddAsync(transporte);
                await _context.SaveChangesAsync();
                return transporte;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("placa_UNIQUE")) { msg = "La placa ya se encuentra registrada."; }
                else if (msg.Contains("tarjeta_UNIQUE")) { msg = "Nro de tarjeta ya se encuentra registrado"; }
                throw new Exception(msg);
            }
        }
        //Datos envio
        public async Task<DatosEnvio> CreateDatosEnvioAsync(DatosEnvio datosEnvio)
        {
            if (datosEnvio == null)
            {
                throw new ArgumentNullException(nameof(datosEnvio), "El objeto datosEnvio no puede ser nulo");
            }
            await _context.DatosEnvios.AddAsync(datosEnvio);
            await _context.SaveChangesAsync();
            return datosEnvio;
        }
        //Guia detalle
        public async Task<GuiaDetalle> CreateGuiaDetalleAsync(GuiaDetalle guiaDetalle)
        {
            if (guiaDetalle == null)
            {
                throw new ArgumentNullException(nameof(guiaDetalle), "El objeto guiaDetalle no puede ser nulo");
            }
            await _context.GuiaDetalles.AddAsync(guiaDetalle);
            await _context.SaveChangesAsync();
            return guiaDetalle;
        }

        public async Task<Origen> CreateOrigenAsync(Origen origen)
        {
            if (origen == null)
            {
                throw new ArgumentNullException(nameof(origen), "El objeto origen no puede ser nulo");
            }
            await _context.Origens.AddAsync(origen);
            await _context.SaveChangesAsync();
            return origen;
        }

        public async Task<Destino> CreateDestinoAsync(Destino destino)
        {
            if (destino == null)
            {
                throw new ArgumentNullException(nameof(destino), "El objeto destino no puede ser nulo");
            }
            await _context.Destinos.AddAsync(destino);
            await _context.SaveChangesAsync();
            return destino;
        }

    }
}
