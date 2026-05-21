using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.VentaConfig;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia;
using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<VentaConfig, VentaConfigAccessDTO>();
            CreateMap<VentaConfig, EmpresaListDTO>();

            //Guias de remisión
            CreateMap<GuiaRemision, GuiaRemitenteDTO>()
                .ForMember(dest => dest.SerieCorrelativo, opt => opt.MapFrom(src => src.NumeroGuia))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.EmailFacturador, opt => opt.MapFrom(src => src.RucNavigation.EmailPrincipal))
                .ForMember(dest => dest.Remitente, opt => opt.MapFrom(src => src.Remitente))
                .ForMember(dest => dest.Destinatario, opt => opt.MapFrom(src => src.Destinatario))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.DatosEnvio, opt => opt.MapFrom(src => src.DatosEnvios.FirstOrDefault()))
                //Mapear conductor y transporte dentro de DatosEnvio
                //.ForMember(dest => dest.DatosEnvio.ConductorGuia, opt => opt.MapFrom(src => src.DatosEnvios.FirstOrDefault().ConductorGuids))
                //.ForMember(dest=>dest.DatosEnvio.TransporteGuia, opt=>opt.MapFrom(src=>src.DatosEnvios.FirstOrDefault().TransporteGuids))

                .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Origens.FirstOrDefault()))
                .ForMember(dest => dest.Destino, opt => opt.MapFrom(src => src.Destinos))
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.GuiaDetalles))
                .ForMember(dest => dest.AreaNombre, opt => opt.MapFrom(src => src.Area.Descripcion))
                ;
            CreateMap<GuiaRemision, GuiaRemisionListDTO>()
                .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => src.Fecha.ToString("yyyy-MM-dd")))
                //.ForMember(dest => dest.HoraEmision, opt => opt.MapFrom(src => src.Hora.ToString(@"hh\:mm\:ss")))
                .ForMember(dest => dest.HoraEmision, opt => opt.MapFrom(src => src.Hora))
                .ForMember(dest => dest.EntidadRemitente, opt => opt.MapFrom(src => src.Remitente.RucNavigation.Empresa))
                .ForMember(dest => dest.NumeroDocumentoRemitente, opt => opt.MapFrom(src => src.Remitente.Ruc))
                .ForMember(dest => dest.EstablecimientoRemitente, opt => opt.MapFrom(src => src.Remitente.Descripcion))
                .ForMember(dest => dest.EntidadDestinatario, opt => opt.MapFrom(src => src.Destinatario.RucNavigation.Empresa))
                .ForMember(dest => dest.EstablecimientoDestinatario, opt => opt.MapFrom(src => src.Destinatario.Descripcion))
                .ForMember(dest => dest.NumeroDocumentoDestinatario, opt => opt.MapFrom(src => src.Destinatario.Ruc))
                .ForMember(dest => dest.EntidadRemitente, opt => opt.MapFrom(src => src.RucNavigation.Empresa))
                .ForMember(dest => dest.DistritoOrigen, opt => opt.MapFrom(src => src.Origens.FirstOrDefault().Ubigeo.Distrito))
                .ForMember(dest => dest.DireccionOrigen, opt => opt.MapFrom(src => src.Remitente.Direccion))
                .ForMember(dest => dest.DireccionDestino, opt => opt.MapFrom(src => src.Destinatario.Direccion))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
                .ForMember(dest => dest.DistritoDestino, opt => opt.MapFrom(src => src.Destinos.FirstOrDefault().Ubigeo.Distrito))
                .ForMember(dest => dest.AreaNombre, opt => opt.MapFrom(src => src.Area.Descripcion))
                .ForMember(dest => dest.MotivoTrasladoNombre, opt => opt.MapFrom(src => src.MotivoTraslado.Nombre))
                ;
            CreateMap<GuiaRemitenteCreateDTO, GuiaRemision>()
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                    .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                    .ForMember(dest => dest.Destinatario, opt => opt.Ignore())
                    .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                    ;

            CreateMap<GuiaRemitenteUpdateDTO, GuiaRemision>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)))
                .ForMember(dest=>dest.DocRelacionados, opt=>opt.MapFrom(src=>src.DocRelacionado))
                .ForMember(dest=>dest.EmailDestinatarioGuids, opt=>opt.MapFrom(src=>src.EmailDestinatarioGuia))
                .ForMember(dest=>dest.GuiaDetalles, opt=>opt.MapFrom(src=>src.Productos))
                .ForMember(dest=>dest.Destinos, opt=>opt.MapFrom(src=>src.Destino))
                .ForMember(dest => dest.Origens, opt => opt.MapFrom(src => src.Origen == null ? null : new[] { src.Origen }))
                .ForMember(dest => dest.DatosEnvios, opt => opt.MapFrom(src => src.DatosEnvio == null ? null : new[] { src.DatosEnvio }))
                ;
            //GuiaRemision de remisión por ticket
            CreateMap<GuiaRemision, GuiaPorTicketDTO>()
                .ForMember(dest => dest.Ruc, opt => opt.MapFrom(src => src.Remitente.Ruc))
                ;
            CreateMap<DatosEnvioUpdateDTO, GuiaRemisionDatoEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest=>dest.ConductorGuids, opt=>opt.MapFrom(src=>src.ConductorGuid))
                .ForMember(dest=>dest.TransporteGuids, opt=>opt.MapFrom(src=>src.TransporteGuid))
                ;
            CreateMap<DatosEnvioGTUpdateDTO, GuiaRemisionDatoEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest=>dest.ConductorGuids, opt=>opt.MapFrom(src=>src.ConductorGuid))
                .ForMember(dest=>dest.TransporteGuids, opt=>opt.MapFrom(src=>src.TransporteGuid))
                ;
            CreateMap<DocRelacionadoUpdateDTO, DocRelacionado>();
            CreateMap<EmailDestinatarioGuiaUpdateDTO, EmailDestinatarioGuid>();
            CreateMap<EmailRemiteTransportistaGuiaUpdateDTO, EmailRemiteTransportistaGuid>();
            CreateMap<OrigenUpdateDTO, GuiaRemisionOrigen>()
                .ForMember(dest=>dest.UbigeoId, opt=>opt.MapFrom(src=>src.UbigeoId))
                ;
            CreateMap<DestinoUpdateDTO, GuiaRemisionDestino>();
            CreateMap<ProductosUpdateDTO, GuiaRemisionDetalle>();
            
            CreateMap<UnidadTransporteGuiaUpdateDTO, UnidadTransporteGuid>();
            //Guias de transportista
            CreateMap<GuiaRemision, GuiaTransportistaDTO>()
                .ForMember(dest => dest.SerieCorrelativo, opt => opt.MapFrom(src => src.NumeroGuia))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Remitente, opt => opt.MapFrom(src => src.Remitente))
                .ForMember(dest => dest.RemiteTransportista, opt => opt.MapFrom(src => src.RemiteTransportista))
                .ForMember(dest => dest.Destinatario, opt => opt.MapFrom(src => src.Destinatario))
                .ForMember(dest => dest.DatosEnvio, opt => opt.MapFrom(src => src.DatosEnvios.FirstOrDefault()))
                .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Origens.FirstOrDefault()))
                .ForMember(dest => dest.Destino, opt => opt.MapFrom(src => src.Destinos))
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.GuiaDetalles))
                ;
            CreateMap<GuiaTransportistaCreateDTO, GuiaRemision>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                .ForMember(dest => dest.RemiteTransportista, opt => opt.Ignore())
                .ForMember(dest => dest.Destinatario, opt => opt.Ignore());
            CreateMap<GuiaTransportistaUpdateDTO, GuiaRemision>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)))
                .ForMember(dest => dest.DocRelacionados, opt => opt.MapFrom(src => src.DocRelacionado))
                .ForMember(dest => dest.EmailRemiteTransportistaGuids, opt => opt.MapFrom(src => src.EmailRemiteTransportistaGuia))
                .ForMember(dest => dest.EmailDestinatarioGuids, opt => opt.MapFrom(src => src.EmailDestinatarioGuia))
                .ForMember(dest => dest.GuiaDetalles, opt => opt.MapFrom(src => src.Productos))
                .ForMember(dest => dest.Destinos, opt => opt.MapFrom(src => src.Destino))
                .ForMember(dest => dest.Origens, opt => opt.MapFrom(src => src.Origen == null ? null : new[] { src.Origen }))
                .ForMember(dest => dest.DatosEnvios, opt => opt.MapFrom(src => src.DatosEnvio == null ? null : new[] { src.DatosEnvio }))
                ;

            //Documentos de referencia relacionados a la guía de remisión
            CreateMap<DocRelacionado, DocRelacionadoDTO>();
            CreateMap<DocRelacionadoCreateDTO, DocRelacionado>();

            //Email de destinatarios dentro de Guía de Remisión
            CreateMap<EmailDestinatarioGuid, EmailDestinatarioGuiaDTO>();
            CreateMap<EmailDestinatarioGuiaCreateDTO, EmailDestinatarioGuid>();
            CreateMap<string, EmailDestinatarioGuid>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src));
            //.ForMember(dest => dest.GuiaRemisionId, opt => opt.Ignore());

            //Email de remitente transportista dentro de Guía de Remisión
            CreateMap<EmailRemiteTransportistaGuid, EmailRemiteTransportistaGuiaDTO>();
            CreateMap<EmailRemiteTransportistaGuiaCreateDTO, EmailRemiteTransportistaGuid>();
            CreateMap<string, EmailRemiteTransportistaGuid>()
                .ForMember(dest=>dest.Email, opt=>opt.MapFrom(src=>src));

            //Datos de envío para guía remitente
            CreateMap<GuiaRemisionDatoEnvio, DatosEnvioDTO>()
                //.ForMember(dest => dest.ConductorGuia, opt => opt.MapFrom(src => src.ConductorGuids))
                .ForMember(dest => dest.ConductorGuia, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (src?.ConductorGuids == null) return new List<ConductorGuiaDTO>();
                    return src.ConductorGuids
                        .Select((c, i) =>
                        {
                            var dto = context.Mapper.Map<ConductorGuiaDTO>(c);
                            dto.JobTitle = i == 0 ? "Principal" : "Secundario";
                            return dto;
                        })
                        .ToList();
                }))
                .ForMember(dest => dest.TransporteGuia, opt => opt.MapFrom(src => src.TransporteGuids))
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => src.FechaEnvio.ToString("yyyy-MM-dd")))
                ;

            CreateMap<DatosEnvioCreateDTO, GuiaRemisionDatoEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)));
            //Datos de envío para guía transportista
            CreateMap<GuiaRemisionDatoEnvio, DatosEnvioGT_DTO>()
                //.ForMember(dest => dest.ConductorGuia, opt => opt.MapFrom(src => src.ConductorGuids))
                .ForMember(dest => dest.ConductorGuia, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    if (src?.ConductorGuids == null) return new List<ConductorGuiaDTO>();
                    return src.ConductorGuids
                        .Select((c, i) =>
                        {
                            var dto = context.Mapper.Map<ConductorGuiaDTO>(c);
                            dto.JobTitle = i == 0 ? "Principal" : "Secundario";
                            return dto;
                        })
                        .ToList();
                }))
                .ForMember(dest => dest.TransporteGuia, opt => opt.MapFrom(src => src.TransporteGuids))
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => src.FechaEnvio.ToString("yyyy-MM-dd")))
                ;
            CreateMap<DatosEnvioGTCreateDTO, GuiaRemisionDatoEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)));
            //Productos
            CreateMap<GuiaRemisionDetalle, ProductosDTO>()
                .ForMember(dest => dest.DescripcionUM, opt => opt.MapFrom(src => src.UnidadMedida.Descripcion));
            CreateMap<ProductosCreateDTO, GuiaRemisionDetalle>();
            

            //Ubicación origen
            CreateMap<GuiaRemisionOrigen, OrigenDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<OrigenCreateDTO, GuiaRemisionOrigen>();
            //Ubicación destino
            CreateMap<GuiaRemisionDestino, DestinoDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<DestinoCreateDTO, GuiaRemisionDestino>();

            //Ubigeo
            CreateMap<Ubigeo, UbigeoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UbigeoId))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Distrito))
                ;
            //ConductorGuia
            CreateMap<ConductorGuid, ConductorGuiaDTO>()
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.Conductor.TipoDocumento))
                .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.Conductor.NumeroDocumento))
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Conductor.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Conductor.Apellidos))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Conductor.Cargo))
                .ForMember(dest => dest.Licencia, opt => opt.MapFrom(src => src.Conductor.Licencia))
                ;
            CreateMap<ConductorCreateGuiaDTO, ConductorGuid>();
            CreateMap<int, ConductorGuid>()
            .ForMember(dest => dest.ConductorId, opt => opt.MapFrom(src => src));
            //TransporteGuia
            CreateMap<UnidadTransporteGuid, UnidadTransporteGuiaDTO>()
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Transporte.Placa))
                .ForMember(dest => dest.Tarjeta, opt => opt.MapFrom(src => src.Transporte.Tarjeta))
                .ForMember(dest => dest.NroAutorizacion, opt => opt.MapFrom(src => src.Transporte.NroAutorizacion))
                .ForMember(dest => dest.CodEmisorVehicular, opt => opt.MapFrom(src => src.Transporte.CodEmisorVehicular))
                .ForMember(dest => dest.EmisorVehicular, opt => opt.MapFrom(src => src.Transporte.EmisorVehicular))
                ;
            CreateMap<UnidadTransporteCreateDTO, UnidadTransporte>();
            CreateMap<int, UnidadTransporteGuid>()
            .ForMember(dest => dest.TransporteId, opt => opt.MapFrom(src => src));
        }
    }
}
