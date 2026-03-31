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

            }
            catch (Exception ex)
            {
                throw;
            }

            var query = _context.GuiaRemisions.AsQueryable();
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
                            x.Destinatario.RazonSocial.ToLower().Contains(val) || x.Destinatario.NumeroDocumento.Contains(val) ||
                            x.Origens.FirstOrDefault().Ubigeo.Distrito.ToLower().Contains(val) ||
                            x.Destinos.FirstOrDefault().Ubigeo.Distrito.ToLower().Contains(val) ||
                            x.EmpleadoNombreCreacion.ToLower().Contains(val) || x.EmpleadoNombreEdicion.ToLower().Contains(val) ||
                            x.Estado.Contains(val)
                            );
                            break;
                    }
                }
            }
            var totalRecords = await query.CountAsync();
            if (!export)
            {
                query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            }
            var data = await query
                .OrderByDescending(x => x.FechaCreacion)
                //.Skip((pageNumber - 1) * pageSize)
                //.Take(pageSize)
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
                    Remitente = new Remitente { Descripcion = g.Remitente.Descripcion },
                    Destinatario = new Destinatario
                    {
                        TipoDocumento = g.Destinatario.TipoDocumento,
                        NumeroDocumento = g.Destinatario.NumeroDocumento,
                        RazonSocial = g.Destinatario.RazonSocial
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
                    IdEstado = g.IdEstado
                })
                .AsNoTracking()
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<(List<GuiaRemision>, int)> GetListAsync2(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query = _context.GuiaRemisions.AsQueryable();

            if (request.Columns != null && request.Columns.Any())
            {
                foreach (var col in request.Columns)
                {
                    var val = col.Search.Value;
                    var key = col.Data?.ToLower();

                    if (string.IsNullOrEmpty(val)) continue;

                    if (key.Contains("fecha") && val.Contains("|"))
                    {
                        var parts = val.Split('|');
                        if (DateTime.TryParse(parts[0], out var dIni) && DateTime.TryParse(parts[1], out var dFin))
                        {
                            var dFinSiguiente = dFin.AddDays(1);
                            query = query.Where(x => x.FechaCreacion >= dIni && x.FechaCreacion < dFinSiguiente);
                        }
                        continue;
                    }

                    if ((key.Contains("id") || key.Contains("ruc")) && val.Contains(","))
                    {
                        var idsStrings = val.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        if (key == "id_remitente")
                        {
                            var idsInt = idsStrings.Select(int.Parse).ToList();
                            query = query.Where(x => idsInt.Contains(x.RemitenteId));
                        }
                        else if (key == "ruc")
                        {
                            query = query.Where(x => idsStrings.Contains(x.Ruc));
                        }
                        continue;
                    }

                    // Filtros unitarios normales
                    switch (key)
                    {
                        case "ruc": query = query.Where(x => x.Ruc == val); break;
                        case "tipo_guia": query = query.Where(x => x.TipoGuia.Contains(val)); break;
                        case "numero_guia": query = query.Where(x => x.NumeroGuia == val); break;
                        case "serie_guia": query = query.Where(x => x.Serie == val); break;
                        case "id_remitente":
                            if (int.TryParse(val, out int idR)) query = query.Where(x => x.RemitenteId == idR);
                            break;
                    }
                }
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<(List<GuiaRemision>, int)> GetListAsync1(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query = _context.GuiaRemisions
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
                    Remitente = new Remitente
                    {
                        Descripcion = g.Remitente.Descripcion
                    },
                    Destinatario = new Destinatario
                    {
                        TipoDocumento = g.Destinatario.TipoDocumento,
                        NumeroDocumento = g.Destinatario.NumeroDocumento,
                        RazonSocial = g.Destinatario.RazonSocial
                    },
                    Proveedor = g.Proveedor == null ? null : new Proveedor
                    {
                        TipoDocumento = g.Proveedor.TipoDocumento,
                        NumeroDocumento = g.Proveedor.NumeroDocumento,
                        RazonSocial = g.Proveedor.RazonSocial
                    },
                    Origens = g.Origens.Select(o => new Origen
                    {
                        Ubigeo = o.Ubigeo
                    }).ToList(),
                    Destinos = g.Destinos.Select(d => new Destino
                    {
                        Ubigeo = d.Ubigeo
                    }).ToList(),
                    FechaCreacion = g.FechaCreacion,
                    EmpleadoNombreCreacion = g.EmpleadoNombreCreacion,
                    FechaEdicion = g.FechaEdicion,
                    EmpleadoNombreEdicion = g.EmpleadoNombreEdicion,
                    Estado = g.Estado,
                    IdEstado = g.IdEstado
                })
                 .OrderByDescending(x => x.FechaCreacion);

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
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
                    Remitente = new Remitente
                    {
                        RemitenteId = g.Remitente.RemitenteId,
                        Ruc = g.Remitente.Ruc,
                        Descripcion = g.Remitente.Descripcion,
                        UbigeoId = g.Remitente.UbigeoId,
                        Ubigeo = g.Remitente.Ubigeo,
                        Direccion = g.Remitente.Direccion,
                        Email = g.Remitente.Email,
                        Pais = g.Remitente.Pais,
                        Serie = g.Remitente.Serie,
                        CodigoSunat = g.Remitente.CodigoSunat,
                        FechaCreacion = g.Remitente.FechaCreacion,
                        FechaEdicion = g.Remitente.FechaEdicion,
                        EmpleadoNombreCreacion = g.Remitente.EmpleadoNombreCreacion,
                        EmpleadoNombreEdicion = g.Remitente.EmpleadoNombreEdicion,
                        Estado = g.Remitente.Estado,
                        IdEstado = g.Remitente.IdEstado
                    },
                    Destinatario = new Destinatario
                    {
                        DestinatarioId = g.Destinatario.DestinatarioId,
                        TipoDocumento = g.Destinatario.TipoDocumento,
                        NumeroDocumento = g.Destinatario.NumeroDocumento,
                        RazonSocial = g.Destinatario.RazonSocial,
                        UbigeoId = g.Destinatario.UbigeoId,
                        Ubigeo = g.Destinatario.Ubigeo,
                        Direccion = g.Destinatario.Direccion,
                        Pais = g.Destinatario.Pais,
                        CodigoSunat = g.Destinatario.CodigoSunat,
                        FechaCreacion = g.Destinatario.FechaCreacion,
                        EmpleadoNombreCreacion = g.Destinatario.EmpleadoNombreCreacion,
                        FechaEdicion = g.Destinatario.FechaEdicion,
                        EmpleadoNombreEdicion = g.Destinatario.EmpleadoNombreEdicion,
                        EmailDestinatarioGuids = g.EmailDestinatarioGuids,
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
                        FechaCreacion = g.Proveedor.FechaCreacion,
                        EmpleadoNombreCreacion = g.Proveedor.EmpleadoNombreCreacion,
                        FechaEdicion = g.Proveedor.FechaEdicion,
                        EmpleadoNombreEdicion = g.Proveedor.EmpleadoNombreEdicion,
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
                        MotivoEnvio = env.MotivoEnvio,
                        FechaEnvio = env.FechaEnvio,
                        PesoBruto = env.PesoBruto,
                        CodigoUm = env.CodigoUm,
                        RucEmpresaCurrier = env.RucEmpresaCurrier,
                        RazonSocialCurrier = env.RazonSocialCurrier,
                        RegistroMtcCurrier = env.RegistroMtcCurrier,
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
                        TransporteGuids = env.TransporteGuids.Select(tg => new TransporteGuid
                        {
                            TransporteGuidId = tg.TransporteGuidId,
                            DatosEnvioId = tg.DatosEnvioId,
                            TransporteId = tg.TransporteId,
                            Transporte = new Transporte
                            {
                                TransporteId = tg.Transporte.TransporteId,
                                Placa = tg.Transporte.Placa,
                                Tarjeta = tg.Transporte.Tarjeta,
                                NumeroRegistroMtc = tg.Transporte.NumeroRegistroMtc
                            }
                        }).ToList(),
                    }).ToList(),
                    Origens = g.Origens.Select(o => new Origen
                    {
                        UbigeoId = o.UbigeoId,
                        Direccion = o.Direccion,
                        Ubigeo = o.Ubigeo,
                        Pais = o.Pais
                    }).ToList(),
                    Destinos = g.Destinos.Select(d => new Destino
                    {
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
        public async Task<int> CreateGuiaRemisionTransactionalAsync(
            Remitente remitente,
            Destinatario destinatario,
            Proveedor proveedor,
            List<Conductor> conductor,
            List<Transporte> transporte,
            GuiaRemision guiaRemision,
            DatosEnvio datosEnvio,
            Origen origen,
            //Destino destino,
            List<Destino> destino,
            List<GuiaDetalle> detalles,
            List<DocRelacionado> docRelacionados,
            List<EmailDestinatarioGuid> emailDestinatarioGuia
            )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remitente
                var existingRemitente = await GetRemitenteByNameAsync(remitente.Descripcion);
                if (existingRemitente == null)
                {
                    //remitente.EmpleadoIdcreacion= guiaRemision.EmpleadoIdcreacion;
                    //remitente.EmpleadoNombreCreacion=guiaRemision.EmpleadoNombreCreacion;
                    //await _context.Remitentes.AddAsync(remitente);
                    //await _context.SaveChangesAsync();
                    throw new Exception("No existe este remitente.");
                }
                else
                {
                    remitente = existingRemitente;
                }

                // Destinatario
                var existingDestinatario = await GetDestinatarioByRucAsync(destinatario.NumeroDocumento);
                if (existingDestinatario == null)
                {
                    destinatario.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                    destinatario.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                    //await _context.Destinatarios.AddAsync(destinatario);
                    //await _context.SaveChangesAsync();
                    await CreateDestinatarioAsync(destinatario);
                }
                else
                {
                    destinatario = existingDestinatario;
                    await UpdateDestinatarioAsync(destinatario);
                }

                // Proveedor
                if (guiaRemision.TipoTraslado == "COMPRA")
                {
                    var existingProveedor = await GetProveedorByRucAsync(proveedor.NumeroDocumento);
                    if (existingProveedor == null)
                    {
                        proveedor.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                        proveedor.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                        //await _context.Proveedors.AddAsync(proveedor);
                        //await _context.SaveChangesAsync();
                        await CreateProveedorAsync(proveedor);
                    }
                    else
                    {
                        proveedor = existingProveedor;
                    }
                }


                // Guía
                guiaRemision.RemitenteId = remitente.RemitenteId;
                guiaRemision.Ruc = remitente.Ruc;
                guiaRemision.DestinatarioId = destinatario.DestinatarioId;
                guiaRemision.Estado = "CREADO";
                guiaRemision.IdEstado = 0;
                if (guiaRemision.TipoTraslado == "COMPRA") { guiaRemision.ProveedorId = proveedor.ProveedorId; }

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
                guiaRemision.RespuestaTicket = "tc11446_no_enviado_REMITENTE";
                await _context.GuiaRemisions.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaId = guiaRemision.GuiaId;
                //if (guiaRemision.TipoTransporte == "PRIVADO")
                //{
                //    datosEnvio.ConductorId = conductor.ConductorId;
                //    datosEnvio.TransporteId = transporte.TransporteId;
                //}
                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // Conductor
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    if (guiaRemision.TipoTransporte == "PRIVADO")
                    {
                        var existingConductor = await GetConductorByNumeroDocumentoAsync(condu.NumeroDocumento);
                        if (existingConductor == null)
                        {
                            condu.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                            condu.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                            //await _context.Conductors.AddAsync(condu);
                            //await _context.SaveChangesAsync();
                            await CreateConductorAsync(condu);
                        }
                        else
                        {
                            condu.ConductorId = existingConductor.ConductorId;
                        }
                        var conductorGuia = new ConductorGuid
                        {
                            ConductorId = condu.ConductorId,
                            DatosEnvioId = datosEnvio.DatosEnvioId
                        };
                        await _context.ConductorGuids.AddAsync(conductorGuia);
                        await _context.SaveChangesAsync();
                    }
                }

                // Transporte
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    if (guiaRemision.TipoTransporte == "PRIVADO")
                    {
                        var existingTransporte = await GetTransporteByPlacaAsync(tr.Placa);
                        if (existingTransporte == null)
                        {
                            tr.EmpleadoIdcreacion = guiaRemision.EmpleadoIdcreacion;
                            tr.EmpleadoNombreCreacion = guiaRemision.EmpleadoNombreCreacion;
                            //await _context.Transportes.AddAsync(tr);
                            //await _context.SaveChangesAsync();
                            await CreateTransporteAsync(tr);
                        }
                        else
                        {
                            tr.TransporteId = existingTransporte.TransporteId;
                        }
                        var transporteGuia = new TransporteGuid
                        {
                            TransporteId = tr.TransporteId,
                            DatosEnvioId = datosEnvio.DatosEnvioId
                        };
                        await _context.TransporteGuids.AddAsync(transporteGuia);
                        await _context.SaveChangesAsync();
                    }
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
                        email.DestinatarioId = destinatario.DestinatarioId;
                        await _context.EmailDestinatarioGuids.AddAsync(email);
                    }
                    await _context.SaveChangesAsync();
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

                //retornar el ID de la guía creada
                return guiaRemision.GuiaId;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estado, int idEstado)
        {
            var guia = await _context.GuiaRemisions.FindAsync(guiaId);
            if (guia == null)
                throw new KeyNotFoundException("Guía no encontrada");

            guia.RespuestaTicket = ticket;
            guia.Estado = estado;
            guia.IdEstado = (sbyte)idEstado;
            guia.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(GuiaRemision guiaRemision)
        {
            if (guiaRemision == null)
            {
                throw new ArgumentNullException(nameof(guiaRemision), "El objeto guiaRemision no puede ser nulo");
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
        //Remitente
        public async Task<Remitente> GetRemitenteByNameAsync(string name)
        {
            return await _context.Remitentes.FirstOrDefaultAsync(r => r.Descripcion == name);
        }
        public async Task<Remitente> CreateRemitenteAsync(Remitente remitente)
        {
            try
            {
                if (remitente == null)
                {
                    throw new ArgumentNullException(nameof(remitente), "El objeto remitente no puede ser nulo");
                }
                await _context.Remitentes.AddAsync(remitente);
                await _context.SaveChangesAsync();
                return remitente;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("razonSocial_UNIQUE")) { msg = "La razón social social o sucursal del remitente ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        //Destinatario
        public async Task<Destinatario> GetDestinatarioByRucAsync(string ruc)
        {
            return await _context.Destinatarios.FirstOrDefaultAsync(d => d.NumeroDocumento == ruc);
        }
        public async Task<Destinatario> CreateDestinatarioAsync(Destinatario destinatario)
        {
            try
            {
                if (destinatario == null)
                {
                    throw new ArgumentNullException(nameof(destinatario), "El objeto destinatario no puede ser nulo");
                }
                await _context.Destinatarios.AddAsync(destinatario);
                await _context.SaveChangesAsync();
                return destinatario;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
        }
        public async Task<Destinatario> UpdateDestinatarioAsync(Destinatario destinatario)
        {
            try
            {
                _context.Destinatarios.Update(destinatario);
                destinatario.FechaEdicion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return destinatario;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("numeroDocumento_UNIQUE")) { msg = "El número de documento ya se encuentra registrado."; }
                throw new Exception(msg);
            }
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
        public async Task<Transporte> GetTransporteByPlacaAsync(string placa)
        {
            return await _context.Transportes.FirstOrDefaultAsync(t => t.Placa == placa);
        }
        public async Task<Transporte> CreateTransporteAsync(Transporte transporte)
        {
            try
            {
                if (transporte == null)
                {
                    throw new ArgumentException(nameof(transporte), "El objeto transporte no puede ser nulo");
                }
                await _context.Transportes.AddAsync(transporte);
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
