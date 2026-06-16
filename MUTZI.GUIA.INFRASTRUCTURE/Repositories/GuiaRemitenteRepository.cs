using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Enums;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemitenteRepository : IGuiaRemitenteRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemitenteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<(List<GuiaRemision>, int)> GetListAsync(int pageNumber, int pageSize, Dictionary<string, string> filters, bool export = false)
        {
            try
            {
                var query = _context.GuiaRemision
                    .AsQueryable();

                foreach(var filter in filters)
                {
                    var key = filter.Key;
                    var val = filter.Value.ToLower();

                    switch (key)
                    {
                        case "fecha_registro":
                        {
                            var parts = val.Split('|', StringSplitOptions.RemoveEmptyEntries);
                            if (
                                parts.Length == 2 &&
                                DateTime.TryParse(parts[0], out var iniDt) &&
                                DateTime.TryParse(parts[1], out var finDt))
                            {
                                var ini = DateOnly.FromDateTime(iniDt);
                                var fin = DateOnly.FromDateTime(finDt);
                                query = query.Where(x =>
                                    DateOnly.FromDateTime(x.FechaRegistro) >= ini &&
                                    DateOnly.FromDateTime(x.FechaRegistro) <= fin);
                            }
                            break;
                         }

                        case "fecha_emision":
                        {
                            var parts = val.Split('|', StringSplitOptions.RemoveEmptyEntries);
                            if (
                                parts.Length == 2 &&
                                DateTime.TryParse(parts[0], out var iniDt) &&
                                DateTime.TryParse(parts[1], out var finDt))
                            {
                                var ini = DateOnly.FromDateTime(iniDt);
                                var fin = DateOnly.FromDateTime(finDt);
                                query = query.Where(x =>
                                    DateOnly.FromDateTime(x.FechaRegistro) >= ini &&
                                    DateOnly.FromDateTime(x.FechaRegistro) <= fin);
                            }
                            break;
                        }

                        case "remitente":
                        {
                            query = query.Where(x => x.Remitente.Ruc == val);
                            break;
                        }

                        case "remitente_id":
                        {
                            var ids = val.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(int.Parse)
                                            .ToList();

                            if (ids.Count == 1)
                                query = query.Where(x => x.RemitenteId == ids[0]);
                            else if (ids.Count > 1)
                                query = query.Where(x => ids.Contains(x.RemitenteId));

                            break;
                        }

                        case "destinatario":
                        {
                            query = query.Where(x => x.Destinatario.Ruc == val);
                            break;
                        }

                        case "destinatario_id":
                        {
                            var ids = val.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(int.Parse)
                                            .ToList();

                            if (ids.Count == 1)
                                query = query.Where(x => x.DestinatarioId == ids[0]);
                            else if (ids.Count > 1)
                                query = query.Where(x => ids.Contains(x.DestinatarioId));

                            break;
                        }

                        case "serie_guia":
                        {
                            query = query.Where(x => x.Serie == val);
                            break;
                        }

                        case "correlativo":
                        {
                            if (int.TryParse(val, out var correlativoInt))
                            {
                                query = query.Where(x => int.Parse(x.Correlativo) == correlativoInt);
                            }
                            break;
                        }

                        case "motivo_traslado_id":
                        {
                            var ids = val.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(int.Parse)
                                            .ToList();

                            if (ids.Count == 1)
                                query = query.Where(x => x.MotivoTrasladoId == ids[0]);
                            else if (ids.Count > 1)
                                query = query.Where(x => ids.Contains(x.MotivoTrasladoId));

                            break;
                        }

                        case "tipo_transporte":
                        {
                            query = query.Where(x => x.TipoTransporte == val);
                            break;
                        }

                        case "estado_id":
                        {
                            var ids = val.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(int.Parse)
                                            .ToList();

                            if (ids.Count == 1)
                                query = query.Where(x => x.EstadoId == ids[0]);
                            else if (ids.Count > 1)
                                query = query.Where(x => ids.Contains(x.EstadoId));

                            break;
                        }

                        case "estado_sunat":
                        {
                            var ids = val.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .ToList();

                            if (ids.Count == 1)
                                query = query.Where(x => x.EstadoSunat == ids[0]);
                            else if (ids.Count > 1)
                                query = query.Where(x => ids.Contains(x.EstadoSunat));

                            break;
                        }

                        case "search":
                        {
                            query = query.Where(x => 
                                x.Remitente.Descripcion.ToLower().Contains(val) ||
                                x.Remitente.Ruc.ToLower().Contains(val) ||
                                x.Destinatario.Descripcion.ToLower().Contains(val) ||
                                x.Destinatario.Ruc.ToLower().Contains(val) ||
                                x.NumeroGuia.Contains(val) ||
                                x.MotivoTraslado.Nombre.ToLower().Contains(val) ||
                                x.TipoTransporte.ToLower().Contains(val) ||
                                x.Origens.FirstOrDefault()!.Ubigeo.Distrito!.ToLower().Contains(val) ||
                                x.Destinos.FirstOrDefault()!.Ubigeo.Distrito!.ToLower().Contains(val) ||
                                x.UsuarioRegistro.ToLower().Contains(val) ||
                                (x.UsuarioModifico ?? "").ToLower().Contains(val) ||
                                x.Estado.Nombre.Contains(val) ||
                                x.EstadoSunat.Contains(val)
                            );
                            break;
                        }
                        default: break;
                    }

                }

                query = query.OrderByDescending(x => x.GuiaRemisionId);

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
                        GuiaRemisionId = g.GuiaRemisionId,
                        GuiaRemisionUuid = g.GuiaRemisionUuid,
                        TipoGuia = g.TipoGuia,
                        Ruc = g.Ruc,
                        RucNavigation = new VentaConfig
                        {
                            EmailPrincipal = g.RucNavigation.EmailPrincipal,
                            Empresa = g.RucNavigation.Empresa
                        },
                        TipoTransporte = g.TipoTransporte,
                        MotivoTraslado = g.MotivoTraslado,
                        NumeroGuia = g.NumeroGuia,
                        Serie = g.Serie,
                        Correlativo = g.Correlativo,
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
                        Origens = g.Origens.Select(o => new GuiaRemisionOrigen { Ubigeo = o.Ubigeo }).ToList(),
                        Destinos = g.Destinos.Select(d => new GuiaRemisionDestino { Ubigeo = d.Ubigeo }).ToList(),
                        FechaRegistro = g.FechaRegistro,
                        UsuarioRegistro = g.UsuarioRegistro,
                        UsuarioRegistroNombre = g.UsuarioRegistroNombre,
                        FechaModifico = g.FechaModifico,
                        UsuarioModifico = g.UsuarioModifico,
                        UsuarioModificoNombre = g.UsuarioModificoNombre,
                        Estado = g.Estado,
                        EstadoId = g.EstadoId,
                        EstadoSunat = g.EstadoSunat,
                        EstadoSunatId = g.EstadoSunatId,
                        AreaId = g.AreaId,
                        Area = new Area {
                            Descripcion = g.Area.Descripcion
                        }
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
            return await _context.GuiaRemision
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
                    .ThenInclude(de => de.GuiaRemisionConductores)
                        .ThenInclude(con => con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.GuiaRemisionUnidadesTransporte)
                        .ThenInclude(tr => tr.UnidadTransporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.UnidadMedida)
                .Include(doc => doc.DocRelacionados)
                .Include(doc => doc.Estado)
                .Include(doc => doc.Area)
                //.Include(e=>e.EmailDestinatarioGuids)
                .ToListAsync();
        }
        public async Task<GuiaRemision> GetByTicketAsync(string ticket)
        {
            return await _context.GuiaRemision.Include(r => r.Remitente)
                .FirstOrDefaultAsync(g => g.RespuestaTicket == ticket);
        }

        public async Task<GuiaRemision?> GetByUuidAsync(Guid uuid)
        {
            try
            {
                var guiaRemision = await _context.GuiaRemision
                .Include(g => g.MotivoTraslado)

                .Include(g => g.Remitente)
                    .ThenInclude(h => h.Ubigeo)
                .Include(g => g.Destinatario)
                    .ThenInclude(h => h.Ubigeo)
                .Include(g => g.RemiteTransportista)
                .Include(g => g.RucNavigation)

                .Include(g => g.Proveedor)
                    .ThenInclude(h => h.TipoDocumento)
                .Include(g => g.Proveedor)
                    .ThenInclude(h => h.Ubigeo)


                .Include(g => g.GuiaDetalles)
                    .ThenInclude(h => h.UnidadMedida)


                .Include(g => g.DatosEnvios)
                    .ThenInclude(h => h.UnidadMedida)
                .Include(g => g.DatosEnvios)
                    .ThenInclude(h => h.Transportista)
                .Include(g => g.DatosEnvios)
                    .ThenInclude(h => h.GuiaRemisionConductores)
                        .ThenInclude(h => h.Conductor)
                            .ThenInclude(h => h.TipoDocumento)
                .Include(g => g.DatosEnvios)
                    .ThenInclude(h => h.GuiaRemisionUnidadesTransporte)
                        .ThenInclude(h => h.UnidadTransporte)
                            .ThenInclude(h => h.EntidadReguladoraVehicular)


                .Include(g => g.Origens)
                .Include(g => g.Destinos)
                .Include(g => g.Area)
                .Where(g => g.GuiaRemisionUuid == uuid)
                .FirstOrDefaultAsync();

                return guiaRemision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GuiaRemision?> GetByIdAsync(int id)
        {
            try
            {
                var guiaRemision = await _context.GuiaRemision
                //.Include(g => g.Remitente)
                //.Include(g => g.Destinatario)
                //.Include(g => g.RemiteTransportista)
                //.Include(g => g.RucNavigation)
                //.Include(g => g.Proveedor)
                //.Include(g => g.GuiaDetalles)
                //.Include(g => g.DatosEnvios)
                //.Include(g => g.Origens)
                .Include(g => g.DocRelacionados)
                .Where(g => g.GuiaRemisionId == id)
                .Select(g => new GuiaRemision
                {
                   

                    GuiaRemisionId = g.GuiaRemisionId,
                    GuiaRemisionUuid = g.GuiaRemisionUuid,
                    TipoGuia = g.TipoGuia,
                    Ruc = g.Ruc,
                    RucNavigation = new VentaConfig
                    {
                        EmailPrincipal = g.RucNavigation.EmailPrincipal
                    },

                    MotivoTrasladoId = g.MotivoTrasladoId,
                    MotivoTraslado = g.MotivoTraslado,


                    TipoTransporte = g.TipoTransporte,
                    
                    NumeroGuia = g.NumeroGuia,
                    Fecha = g.Fecha,
                    Hora = g.Hora,
                    Observacion = g.Observacion,
                    
                    
                    RemiteTransportista = g.RemiteTransportista == null ? null : new Transportista
                    {
                        TransportistaId = g.RemiteTransportista.TransportistaId,
                        TipoDocumento = g.RemiteTransportista.TipoDocumento,
                        NumeroDocumento = g.RemiteTransportista.NumeroDocumento,
                        RazonSocial = g.RemiteTransportista.RazonSocial,
                        UbigeoId = g.RemiteTransportista.UbigeoId,
                        Ubigeo= g.RemiteTransportista.Ubigeo,
                        Direccion = g.RemiteTransportista.Direccion,
                        Pais = g.RemiteTransportista.Pais,
                        CodigoSunat = g.RemiteTransportista.CodigoSunat,
                        FechaRegistro = g.RemiteTransportista.FechaRegistro,
                        UsuarioRegistro = g.RemiteTransportista.UsuarioRegistro,
                        FechaModifico = g.RemiteTransportista.FechaModifico,
                        UsuarioModifico = g.RemiteTransportista.UsuarioModifico,
                        EmailRemiteTransportistaGuids = g.EmailRemiteTransportistaGuids,
                        Estado = g.RemiteTransportista.Estado,
                        EstadoId = g.RemiteTransportista.EstadoId
                    },
                    Remitente = new Establecimiento
                    {
                        EstablecimientoId = g.Remitente.EstablecimientoId,
                        Ruc = g.Remitente.Ruc,
                        UbigeoId = g.Remitente.UbigeoId,
                        Ubigeo = g.Remitente.Ubigeo,
                        Direccion = g.Remitente.Direccion,
                        Pais = g.Remitente.Pais,
                        CodigoSunat = g.Remitente.CodigoSunat,
                        FechaRegistro = g.Remitente.FechaRegistro,
                        UsuarioRegistro = g.Remitente.UsuarioRegistro,
                        FechaModifico = g.Remitente.FechaModifico,
                        UsuarioModifico = g.Remitente.UsuarioModifico,
                        Estado = g.Remitente.Estado,
                        EstadoId = g.Remitente.EstadoId,
                        RucNavigation = g.Remitente.RucNavigation
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
                        EstadoId = g.Destinatario.EstadoId,
                        RucNavigation = g.Destinatario.RucNavigation
                    },

                    Proveedor = g.Proveedor == null ? null : new Proveedor
                    {
                        ProveedorId = g.Proveedor.ProveedorId,
                        TipoDocumento = g.Proveedor.TipoDocumento,
                        NumeroDocumento = g.Proveedor.NumeroDocumento,
                        RazonSocial = g.Proveedor.RazonSocial,
                        UbigeoId = g.Proveedor.UbigeoId,
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
                        EstadoId = g.Proveedor.EstadoId
                    },

                    GuiaDetalles = g.GuiaDetalles.Select(det => new GuiaRemisionDetalle
                    {
                        GuiaRemisionDetalleId = det.GuiaRemisionDetalleId,
                        GuiaRemisionId = det.GuiaRemisionId,
                        Cantidad = det.Cantidad,
                        UnidadMedidaId = det.UnidadMedidaId,
                        CodigoUm = det.CodigoUm,
                        Codigo = det.Codigo,
                        Descripcion = det.Descripcion,
                        CodigoSunat = det.CodigoSunat,
                        Gtin = det.Gtin,
                        CodigoSubnacional = det.CodigoSubnacional,
                        IndicadorBienNormalizado = det.IndicadorBienNormalizado,
                        UnidadMedida = det.UnidadMedida
                    }).ToList(),

                    // Datos de Envío y Conductores
                    DatosEnvios = g.DatosEnvios.Select(env => new GuiaRemisionDatoEnvio
                    {
                        GuiaRemisionDatoEnvioId = env.GuiaRemisionDatoEnvioId,
                        MotivoEnvio = env.MotivoEnvio,
                        FechaEnvio = env.FechaEnvio,
                        FechaEntregaTransportista = env.FechaEntregaTransportista,
                        PesoBruto = env.PesoBruto,
                        UnidadMedida = env.UnidadMedida,
                        CodigoUm = env.CodigoUm,

                        IndicadorTrasladoVehiculoCategoria = env.IndicadorTrasladoVehiculoCategoria,
                        TrasladoVehiculoCategoriaPlacaVehiculo = env.TrasladoVehiculoCategoriaPlacaVehiculo,

                        IndicadorRetornoVehiculoEnvasesVacios = env.IndicadorRetornoVehiculoEnvasesVacios,
                        IndicadorRetornoVehiculoVacio = env.IndicadorRetornoVehiculoVacio,
                        IndicadorTransbordoProgramado = env.IndicadorTransbordoProgramado,
                        IndicadorTrasladoTotalDam = env.IndicadorTrasladoTotalDam,
                        IndicadorRegistroVehiculoConductor = env.IndicadorRegistroVehiculoConductor,

                        GuiaRemisionConductores = env.GuiaRemisionConductores.Select(cg => new GuiaRemisionConductor
                        {
                            GuiaRemisionConductorId = cg.GuiaRemisionConductorId,
                            GuiaRemisionDatoEnvioId = cg.GuiaRemisionDatoEnvioId,
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
                        GuiaRemisionUnidadesTransporte = env.GuiaRemisionUnidadesTransporte.Select(tg => new GuiaRemisionUnidadTransporte
                        {
                            GuiaRemisionUnidadTransporteId = tg.GuiaRemisionUnidadTransporteId,
                            GuiaRemisionDatoEnvioId = tg.GuiaRemisionDatoEnvioId,
                            UnidadTransporteId = tg.UnidadTransporteId,
                            UnidadTransporte = new UnidadTransporte
                            {
                                UnidadTransporteId = tg.UnidadTransporte.UnidadTransporteId,
                                Placa = tg.UnidadTransporte.Placa,
                                Tarjeta = tg.UnidadTransporte.Tarjeta,
                                NroAutorizacion = tg.UnidadTransporte.NroAutorizacion,
                                EntidadReguladoraVehicular = tg.UnidadTransporte.EntidadReguladoraVehicular,
                                EmisorVehicular = tg.UnidadTransporte.EntidadReguladoraVehicular.Descripcion,
                                EntidadReguladoraVehicularId = tg.UnidadTransporte.EntidadReguladoraVehicularId,
                            }
                        }).ToList(),

                        TransportistaId = env.TransportistaId,
                        Transportista = env.Transportista
                    }).ToList(),

                    // Puntos de Origen
                    Origens = g.Origens.Select(o => new GuiaRemisionOrigen
                    {
                        GuiaRemisionOrigenId = o.GuiaRemisionOrigenId,
                        UbigeoId = o.UbigeoId,
                        Direccion = o.Direccion,
                        Ubigeo = o.Ubigeo,
                        Pais = o.Pais
                    }).ToList(),

                    // Puntos de Destino
                    Destinos = g.Destinos.Select(d => new GuiaRemisionDestino
                    {
                        GuiaRemisionDestinoId = d.GuiaRemisionDestinoId,
                        UbigeoId = d.UbigeoId,
                        Direccion = d.Direccion,
                        Ubigeo = d.Ubigeo,
                        Pais = d.Pais
                    }).ToList(),
                    DocRelacionados = g.DocRelacionados.ToList(),
                    FechaRegistro = g.FechaRegistro,
                    UsuarioRegistro = g.UsuarioRegistro,
                    Estado = g.Estado,
                    EstadoId = g.EstadoId,
                    Area = new Area
                    {
                        Id = g.Area.Id,
                        Descripcion = g.Area.Descripcion
                    },

                })
                .FirstOrDefaultAsync();

                return guiaRemision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<GuiaRemision> GetByIdAsync2(int id)
        {
            var data = await _context.GuiaRemision
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
                    .ThenInclude(de => de.GuiaRemisionConductores)
                        .ThenInclude(con => con.Conductor)
                .Include(env => env.DatosEnvios)
                    .ThenInclude(de => de.GuiaRemisionUnidadesTransporte)
                        .ThenInclude(tr => tr.UnidadTransporte)
                .Include(det => det.GuiaDetalles)
                    .ThenInclude(gd => gd.UnidadMedida)
                .Include(doc => doc.DocRelacionados)
                .Include(e => e.EmailDestinatarioGuids)
                .Include(erm => erm.EmailRemiteTransportistaGuids)
                .Include(erm => erm.Estado)
                .Include(erm => erm.Area)
                .Include(erm => erm.MotivoTraslado)
                .FirstOrDefaultAsync(g => g.GuiaRemisionId == id);

            return data;
        }

        public async Task<(int GuiaId, string NumeroGuia)> CreateGuiaRemisionTransactionalAsync(
            Establecimiento remitente,
            Establecimiento destinatario,
            Proveedor proveedor,
            List<GuiaRemisionConductor> conductor,
            List<GuiaRemisionUnidadTransporte> transporte,
            GuiaRemision guiaRemision,
            GuiaRemisionDatoEnvio datosEnvio,
            GuiaRemisionOrigen origen,
            List<GuiaRemisionDestino> destino,
            List<GuiaRemisionDetalle> detalles,
            List<GuiaRemisionDocumentoRelacionado> docRelacionados,
            List<GuiaRemisionEmailDestinatario> emailDestinatarioGuia
        )
        {
            //using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Guía
                guiaRemision.RemitenteId = remitente.EstablecimientoId;
                guiaRemision.Ruc = remitente.Ruc;
                guiaRemision.DestinatarioId = destinatario.EstablecimientoId;
                //guiaRemision.Nombre = EnumGuiaRemisionEstado.Registrada.GetDescription();
                guiaRemision.EstadoId = (int)EnumGuiaRemisionEstado.Registrada;
                //if (guiaRemision.TipoTraslado == "COMPRA") { guiaRemision.ProveedorId = proveedor.ProveedorId; }

                //string serie = guiaRemision.Serie ?? "T001";
                string serie = "T" + remitente.Serie.Trim();

                //Buscar el último correlativo para esa serie
                var lastGuia = await _context.GuiaRemision
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
                guiaRemision.RespuestaTicket = "registrada";
                await _context.GuiaRemision.AddAsync(guiaRemision);
                await _context.SaveChangesAsync();

                // Datos Envío
                datosEnvio.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                await _context.DatosEnvios.AddAsync(datosEnvio);
                await _context.SaveChangesAsync();

                // ConductorgUID
                //Registrar en lista
                foreach (var condu in conductor)
                {
                    var conductorGuia = new GuiaRemisionConductor
                    {
                        ConductorId = condu.ConductorId,
                        GuiaRemisionDatoEnvioId = datosEnvio.GuiaRemisionDatoEnvioId
                    };
                    await _context.ConductorGuids.AddAsync(conductorGuia);
                    await _context.SaveChangesAsync();
                }

                // TransportegUID
                //Registrar en lista
                foreach (var tr in transporte)
                {
                    var transporteGuia = new GuiaRemisionUnidadTransporte
                    {
                        UnidadTransporteId = tr.UnidadTransporteId,
                        GuiaRemisionDatoEnvioId = datosEnvio.GuiaRemisionDatoEnvioId
                    };
                    await _context.UnidadTransporteGuids.AddAsync(transporteGuia);
                    await _context.SaveChangesAsync();
                }

                // DocRelacionados
                if (docRelacionados is not null)
                {
                    foreach (var doc in docRelacionados)
                    {
                        doc.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                        await _context.DocRelacionados.AddAsync(doc);
                    }
                    await _context.SaveChangesAsync();
                }

                //EmailDestinatarioGuia
                if (emailDestinatarioGuia is not null)
                {
                    foreach (var email in emailDestinatarioGuia)
                    {
                        email.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                        email.EstablecimientoId = destinatario.EstablecimientoId;
                        await _context.EmailDestinatarioGuids.AddAsync(email);
                    }
                    await _context.SaveChangesAsync();
                }

                // Proveedor
                if(
                    guiaRemision.MotivoTraslado.CodigoSunat == "02" ||  //compra
                    guiaRemision.MotivoTraslado.Nombre == "13" // otros
                ) 
                {
                    guiaRemision.ProveedorId = proveedor.ProveedorId;
                }

                //Origen
                origen.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                await _context.GuiaRemisionOrigen.AddAsync(origen);
                await _context.SaveChangesAsync();

                foreach (var dest in destino)
                {
                    dest.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                    await _context.GuiaRemisionDestino.AddAsync(dest);
                }
                await _context.SaveChangesAsync();

                // Detalles
                foreach (var detalle in detalles)
                {
                    detalle.GuiaRemisionId = guiaRemision.GuiaRemisionId;
                    await _context.GuiaRemisionDetalle.AddAsync(detalle);
                }
                await _context.SaveChangesAsync();

                //await transaction.CommitAsync();

                //retornar el ID de la guía creada y asimismo el número de guía.
                return (guiaRemision.GuiaRemisionId, guiaRemision.NumeroGuia);
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateRespuestaTicketAsync(int guiaId, string ticket, string estadoSunat, int idEstadoSunat)
        {
            var guia = await _context.GuiaRemision.FindAsync(guiaId);
            if (guia == null)
                throw new KeyNotFoundException("Guía no encontrada");

            guia.RespuestaTicket = ticket;
            guia.EstadoSunat = estadoSunat;
            guia.EstadoSunatId = (sbyte)idEstadoSunat;
            //guia.Nombre = estadoSunat;
            //guia.EstadoId = (sbyte)idEstadoSunat;
            guia.FechaModifico = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        //Actualizar guía de remisión remitente
        public async Task<GuiaRemision> UpdateAsync(GuiaRemision guiaRemision)
        {
            if (guiaRemision == null)
            {
                throw new ArgumentNullException(nameof(guiaRemision), "La guía de remisión remitente no puede ser nula");
            }
            //_context.GuiaRemision.Update(guiaRemision);
            await _context.SaveChangesAsync();
            await _context.Entry(guiaRemision).ReloadAsync();

            return guiaRemision;
        }
        public async Task DeleteAsync(int id)
        {
            var guiaRemision = await _context.GuiaRemision.FindAsync(id);
            if (guiaRemision == null)
            {
                throw new KeyNotFoundException("GuiaRemision de remisión no encontrada");
            }
            _context.GuiaRemision.Remove(guiaRemision);
            await _context.SaveChangesAsync();
        }

        public async Task<GuiaRemision?> FindWithDetail(int id)
        {
             var guiaRemision = await _context.GuiaRemision
                .Where(g => g.GuiaRemisionId == id)
                    .Select(g => new GuiaRemision
                    {
                        GuiaRemisionId = g.GuiaRemisionId,
                        GuiaRemisionUuid = g.GuiaRemisionUuid,
                        TipoGuia = g.TipoGuia,
                        Ruc = g.Ruc,
                        RucNavigation = new VentaConfig
                        {
                            EmailPrincipal = g.RucNavigation.EmailPrincipal,
                            Empresa = g.RucNavigation.Empresa
                        },
                        TipoTransporte = g.TipoTransporte,
                        MotivoTraslado = g.MotivoTraslado,
                        NumeroGuia = g.NumeroGuia,
                        Serie = g.Serie,
                        Correlativo = g.Correlativo,
                        Fecha = g.Fecha,
                        Hora = g.Hora,
                        RespuestaTicket = g.RespuestaTicket,
                        Remitente = new Establecimiento
                        {
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
                        Origens = g.Origens.Select(o => new GuiaRemisionOrigen { Ubigeo = o.Ubigeo }).ToList(),
                        Destinos = g.Destinos.Select(d => new GuiaRemisionDestino { Ubigeo = d.Ubigeo }).ToList(),
                        FechaRegistro = g.FechaRegistro,
                        UsuarioRegistro = g.UsuarioRegistro,
                        UsuarioRegistroNombre = g.UsuarioRegistroNombre,
                        FechaModifico = g.FechaModifico,
                        UsuarioModifico = g.UsuarioModifico,
                        UsuarioModificoNombre = g.UsuarioModificoNombre,
                        Estado = g.Estado,
                        EstadoId = g.EstadoId,
                        EstadoSunat = g.EstadoSunat,
                        EstadoSunatId = g.EstadoSunatId,
                        Area = new Area
                        {
                            Id = g.Area.Id,
                            Descripcion = g.Area.Descripcion
                        }
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            return guiaRemision;
        }


        //Proveedor
        public async Task<Proveedor> GetProveedorByRucAsync(string ruc)
        {
            return await _context.Proveedor.FirstOrDefaultAsync(p => p.NumeroDocumento == ruc);
        }
        public async Task<Proveedor> CreateProveedorAsync(Proveedor proveedor)
        {
            try
            {
                if (proveedor == null)
                {
                    throw new ArgumentNullException(nameof(proveedor), "El objeto proveedor no puede ser nulo");
                }
                await _context.Proveedor.AddAsync(proveedor);
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
            return await _context.Conductor.FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
        }
        public async Task<Conductor> CreateConductorAsync(Conductor conductor)
        {
            try
            {
                if (conductor == null)
                {
                    throw new ArgumentNullException(nameof(conductor), "El objeto conductor no puede ser nulo");
                }
                await _context.Conductor.AddAsync(conductor);
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
        //UnidadTransporte
        public async Task<UnidadTransporte> GetTransporteByPlacaAsync(string placa)
        {
            return await _context.UnidadTransporte.FirstOrDefaultAsync(t => t.Placa == placa);
        }
        public async Task<UnidadTransporte> CreateTransporteAsync(UnidadTransporte transporte)
        {
            try
            {
                if (transporte == null)
                {
                    throw new ArgumentException(nameof(transporte), "El objeto transporte no puede ser nulo");
                }
                await _context.UnidadTransporte.AddAsync(transporte);
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
        public async Task<GuiaRemisionDatoEnvio> CreateDatosEnvioAsync(GuiaRemisionDatoEnvio datosEnvio)
        {
            if (datosEnvio == null)
            {
                throw new ArgumentNullException(nameof(datosEnvio), "El objeto datosEnvio no puede ser nulo");
            }
            await _context.DatosEnvios.AddAsync(datosEnvio);
            await _context.SaveChangesAsync();
            return datosEnvio;
        }
        //GuiaRemision detalle
        public async Task<GuiaRemisionDetalle> CreateGuiaDetalleAsync(GuiaRemisionDetalle guiaDetalle)
        {
            if (guiaDetalle == null)
            {
                throw new ArgumentNullException(nameof(guiaDetalle), "El objeto guiaDetalle no puede ser nulo");
            }
            await _context.GuiaRemisionDetalle.AddAsync(guiaDetalle);
            await _context.SaveChangesAsync();
            return guiaDetalle;
        }

        public async Task<GuiaRemisionOrigen> CreateOrigenAsync(GuiaRemisionOrigen origen)
        {
            if (origen == null)
            {
                throw new ArgumentNullException(nameof(origen), "El objeto origen no puede ser nulo");
            }
            await _context.GuiaRemisionOrigen.AddAsync(origen);
            await _context.SaveChangesAsync();
            return origen;
        }

        public async Task<GuiaRemisionDestino> CreateDestinoAsync(GuiaRemisionDestino destino)
        {
            if (destino == null)
            {
                throw new ArgumentNullException(nameof(destino), "El objeto destino no puede ser nulo");
            }
            await _context.GuiaRemisionDestino.AddAsync(destino);
            await _context.SaveChangesAsync();
            return destino;
        }


    }
}
