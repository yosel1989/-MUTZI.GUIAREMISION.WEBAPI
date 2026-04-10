using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destino;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Origen;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Productos;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.UnidadTransporte;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.DatosEnvio;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;
using MUTZI.GUIA.APPLICATION.DTOs.VentaConfig;
using MUTZI.GUIA.DOMAIN.Entities;
using System.Globalization;
using MUTZI.GUIA.APPLICATION.DTOs.Ubigeo;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.DocRelacionado;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.EmailDestinatarioGuia;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.EmailRemiteTransportistaGuia;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<VentaConfig, VentaConfigAccessDTO>();
            CreateMap<VentaConfig, EmpresaListDTO>();

            //Guias de remisión
            CreateMap<DOMAIN.Entities.GuiaRemision, GuiaRemitenteDTO>()
                .ForMember(dest => dest.SerieCorrelativo, opt => opt.MapFrom(src => src.NumeroGuia))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.EmailFacturador, opt => opt.MapFrom(src => src.RucNavigation.EmailPrincipal))
                .ForMember(dest => dest.Remitente, opt => opt.MapFrom(src => src.Remitente))
                .ForMember(dest => dest.Destinatario, opt => opt.MapFrom(src => src.Destinatario))
                .ForMember(dest => dest.DatosEnvio, opt => opt.MapFrom(src => src.DatosEnvios.FirstOrDefault()))
                //Mapear conductor y transporte dentro de DatosEnvio
                //.ForMember(dest => dest.DatosEnvio.ConductorGuia, opt => opt.MapFrom(src => src.DatosEnvios.FirstOrDefault().ConductorGuids))
                //.ForMember(dest=>dest.DatosEnvio.TransporteGuia, opt=>opt.MapFrom(src=>src.DatosEnvios.FirstOrDefault().TransporteGuids))

                .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Origens.FirstOrDefault()))
                .ForMember(dest => dest.Destino, opt => opt.MapFrom(src => src.Destinos))
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.GuiaDetalles))
                ;
            CreateMap<DOMAIN.Entities.GuiaRemision, GuiaListDTO>()
                .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => src.Fecha.ToString("yyyy-MM-dd")))
                //.ForMember(dest => dest.HoraEmision, opt => opt.MapFrom(src => src.Hora.ToString(@"hh\:mm\:ss")))
                .ForMember(dest => dest.HoraEmision, opt => opt.MapFrom(src => src.Hora))
                .ForMember(dest => dest.RazonRemitente, opt => opt.MapFrom(src => src.Remitente.Descripcion))
                .ForMember(dest => dest.RazonDestinatario, opt => opt.MapFrom(src => src.Destinatario.RazonSocial))
                .ForMember(dest => dest.NroDocumentoDestinatario, opt => opt.MapFrom(src => src.Destinatario.NumeroDocumento))
                .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.RucNavigation.Empresa))
                .ForMember(dest => dest.DistritoOrigen, opt => opt.MapFrom(src => src.Origens.FirstOrDefault().Ubigeo.Distrito))
                .ForMember(dest => dest.DistritoDestino, opt => opt.MapFrom(src => src.Destinos.FirstOrDefault().Ubigeo.Distrito))
                ;
            CreateMap<GuiaRemitenteCreateDTO, DOMAIN.Entities.GuiaRemision>()
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                    .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                    .ForMember(dest => dest.Destinatario, opt => opt.Ignore())
                    .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                    ;

            CreateMap<GuiaRemitenteUpdateDTO, DOMAIN.Entities.GuiaRemision>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)))
                .ForMember(dest=>dest.DocRelacionados, opt=>opt.MapFrom(src=>src.DocRelacionado))
                .ForMember(dest=>dest.EmailDestinatarioGuids, opt=>opt.MapFrom(src=>src.EmailDestinatarioGuia))
                .ForMember(dest=>dest.GuiaDetalles, opt=>opt.MapFrom(src=>src.Productos))
                .ForMember(dest=>dest.Destinos, opt=>opt.MapFrom(src=>src.Destino))
                .ForMember(dest => dest.Origens, opt => opt.MapFrom(src => src.Origen == null ? null : new[] { src.Origen }))
                .ForMember(dest => dest.DatosEnvios, opt => opt.MapFrom(src => src.DatosEnvio == null ? null : new[] { src.DatosEnvio }))
                //.ForMember(dest => dest.Origens.FirstOrDefault(), opt => opt.MapFrom(src => src.Origen))
                //.ForMember(dest=>dest.DatosEnvios.FirstOrDefault(), opt=>opt.MapFrom(src=>src.))
                ;
            //Guia de remisión por ticket
            CreateMap<DOMAIN.Entities.GuiaRemision, GuiaPorTicketDTO>()
                .ForMember(dest => dest.Ruc, opt => opt.MapFrom(src => src.Remitente.Ruc))
                ;
            CreateMap<DatosEnvioUpdateDTO, DOMAIN.Entities.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest=>dest.ConductorGuids, opt=>opt.MapFrom(src=>src.ConductorGuid))
                .ForMember(dest=>dest.TransporteGuids, opt=>opt.MapFrom(src=>src.TransporteGuid))
                ;
            CreateMap<DatosEnvioGTUpdateDTO, DOMAIN.Entities.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                ;
            CreateMap<DocRelacionadoUpdateDTO, DOMAIN.Entities.DocRelacionado>();
            CreateMap<EmailDestinatarioGuiaUpdateDTO, DOMAIN.Entities.EmailDestinatarioGuid>();
            CreateMap<EmailRemiteTransportistaGuiaUpdateDTO, DOMAIN.Entities.EmailRemiteTransportistaGuid>();
            CreateMap<OrigenUpdateDTO, DOMAIN.Entities.Origen>()
                .ForMember(dest=>dest.UbigeoId, opt=>opt.MapFrom(src=>src.UbigeoId))
                ;
            CreateMap<DestinoUpdateDTO, DOMAIN.Entities.Destino>();
            CreateMap<ProductosUpdateDTO, DOMAIN.Entities.GuiaDetalle>();
            CreateMap<ConductorGuiaUpdateDTO, DOMAIN.Entities.ConductorGuid>();
            CreateMap<TransporteGuiaUpdateDTO, DOMAIN.Entities.TransporteGuid>();
            //Guias de transportista
            CreateMap<DOMAIN.Entities.GuiaRemision, GuiaTransportistaDTO>()
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
            CreateMap<GuiaTransportistaCreateDTO, DOMAIN.Entities.GuiaRemision>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                .ForMember(dest => dest.RemiteTransportista, opt => opt.Ignore())
                .ForMember(dest => dest.Destinatario, opt => opt.Ignore());

            //Documentos de referencia relacionados a la guía de remisión
            CreateMap<DOMAIN.Entities.DocRelacionado, DocRelacionadoDTO>();
            CreateMap<DocRelacionadoCreateDTO, DOMAIN.Entities.DocRelacionado>();

            //Email de destinatarios dentro de Guía de Remisión
            CreateMap<DOMAIN.Entities.EmailDestinatarioGuid, EmailDestinatarioGuiaDTO>();
            CreateMap<EmailDestinatarioGuiaCreateDTO, DOMAIN.Entities.EmailDestinatarioGuid>();

            //Email de remitente transportista dentro de Guía de Remisión
            CreateMap<DOMAIN.Entities.EmailRemiteTransportistaGuid, EmailRemiteTransportistaGuiaDTO>();
            CreateMap<EmailRemiteTransportistaGuiaCreateDTO, DOMAIN.Entities.EmailRemiteTransportistaGuid>();

            //Datos de envío para guía remitente
            CreateMap<DOMAIN.Entities.DatosEnvio, DatosEnvioDTO>()
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

            CreateMap<DatosEnvioCreateDTO, DOMAIN.Entities.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)));
            //Datos de envío para guía transportista
            CreateMap<DOMAIN.Entities.DatosEnvio, DatosEnvioGT_DTO>()
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
            CreateMap<DatosEnvioGTCreateDTO, DOMAIN.Entities.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)));
            //Remitente
            CreateMap<DOMAIN.Entities.Remitente, RemitenteDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.NombreEmpresa, opt => opt.MapFrom(src => src.RucNavigation.Empresa))
                ;
            CreateMap<DOMAIN.Entities.Remitente, RemitenteForGuiaDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.NombreEmpresa, opt => opt.MapFrom(src => src.RucNavigation.Empresa))
                ;
            CreateMap<DOMAIN.Entities.Remitente, RemitenteListDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.NombreEmpresa, opt => opt.MapFrom(src => src.RucNavigation.Empresa))
                ;
            CreateMap<DOMAIN.Entities.Remitente, RemitenteNameListDTO>();
            CreateMap<RemitenteCreateDTO, Remitente>();
            CreateMap<RemitenteCreateGuiaDTO, Remitente>();
            CreateMap<RemitenteUpdateDTO, Remitente>();

            //Remite transportista
            CreateMap<DOMAIN.Entities.RemiteTransportistum, RemiteTransportistaDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.EmailRemiteTransportista, opt => opt.MapFrom(src => src.EmailRemiteTransportistaGuids))
                ;
            CreateMap<RemiteTransportistaCreateDTO, DOMAIN.Entities.RemiteTransportistum>();

            //Destinatario
            CreateMap<DOMAIN.Entities.Destinatario, DestinatarioDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.EmailDestinatariosGuia, opt => opt.MapFrom(src => src.EmailDestinatarioGuids))
                ;
            CreateMap<DOMAIN.Entities.Destinatario, DestinatarioListDTO>();
            CreateMap<DestinatarioCreateDTO, DOMAIN.Entities.Destinatario>();
            CreateMap<DestinatarioCreateGuiaDTO, DOMAIN.Entities.Destinatario>();
            CreateMap<DestinatarioUpdateDTO, DOMAIN.Entities.Destinatario>();
            //Proveedor
            CreateMap<DOMAIN.Entities.Proveedor, ProveedorDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<DOMAIN.Entities.Proveedor, ProveedorListDTO>();
            CreateMap<ProveedorCreateDTO, DOMAIN.Entities.Proveedor>();
            CreateMap<ProveedorCreateGuiaDTO, DOMAIN.Entities.Proveedor>();
            CreateMap<ProveedorUpdateDTO, DOMAIN.Entities.Proveedor>();
            //Productos
            CreateMap<DOMAIN.Entities.GuiaDetalle, ProductosDTO>()
                .ForMember(dest => dest.DescripcionUM, opt => opt.MapFrom(src => src.CodigoUmNavigation.Descripcion));
            CreateMap<ProductosCreateDTO, DOMAIN.Entities.GuiaDetalle>();
            //Conductor
            CreateMap<DOMAIN.Entities.Conductor, ConductorDTO>();
            CreateMap<DOMAIN.Entities.Conductor, ConductorListDTO>();
            CreateMap<ConductorCreateDTO, DOMAIN.Entities.Conductor>();
            CreateMap<ConductorUpdateDTO, DOMAIN.Entities.Conductor>();

            //Unidad de transporte
            CreateMap<DOMAIN.Entities.Transporte, UnidadTransporteDTO>();
            CreateMap<DOMAIN.Entities.Transporte, TransporteListDTO>();
            CreateMap<UnidadTransporteCreateDTO, DOMAIN.Entities.Transporte>();
            CreateMap<UnidadTransporteUpdateDTO, DOMAIN.Entities.Transporte>();

            //Ubicación origen
            CreateMap<DOMAIN.Entities.Origen, OrigenDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<OrigenCreateDTO, DOMAIN.Entities.Origen>();
            //Ubicación destino
            CreateMap<DOMAIN.Entities.Destino, DestinoDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<DestinoCreateDTO, DOMAIN.Entities.Destino>();

            //Ubigeo
            CreateMap<Ubigeo, UbigeoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UbigeoId))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Distrito))
                ;
            //ConductorGuia
            CreateMap<DOMAIN.Entities.ConductorGuid, ConductorGuiaDTO>()
                .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.Conductor.TipoDocumento))
                .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.Conductor.NumeroDocumento))
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Conductor.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Conductor.Apellidos))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Conductor.Cargo))
                .ForMember(dest => dest.Licencia, opt => opt.MapFrom(src => src.Conductor.Licencia))
                ;
            CreateMap<ConductorCreateGuiaDTO, DOMAIN.Entities.ConductorGuid>();
            CreateMap<int, DOMAIN.Entities.ConductorGuid>()
            .ForMember(dest => dest.ConductorId, opt => opt.MapFrom(src => src))
            // Opcional: Si quieres ignorar otras propiedades para evitar errores
            .ForMember(dest => dest.ConductorGuidId, opt => opt.Ignore())
            .ForMember(dest => dest.DatosEnvioId, opt => opt.Ignore());
            //TransporteGuia
            CreateMap<DOMAIN.Entities.TransporteGuid, TransporteGuiaDTO>()
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Transporte.Placa))
                .ForMember(dest => dest.Tarjeta, opt => opt.MapFrom(src => src.Transporte.Tarjeta))
                .ForMember(dest => dest.NroAutorizacion, opt => opt.MapFrom(src => src.Transporte.NroAutorizacion))
                .ForMember(dest => dest.CodEmisorVehicular, opt => opt.MapFrom(src => src.Transporte.CodEmisorVehicular))
                .ForMember(dest => dest.EmisorVehicular, opt => opt.MapFrom(src => src.Transporte.EmisorVehicular))
                ;
            CreateMap<UnidadTransporteCreateDTO, DOMAIN.Entities.Transporte>();
            CreateMap<int, DOMAIN.Entities.TransporteGuid>()
            .ForMember(dest => dest.TransporteId, opt => opt.MapFrom(src => src))
            // Opcional: Si quieres ignorar otras propiedades para evitar errores
            .ForMember(dest => dest.TransporteGuidId, opt => opt.Ignore())
            .ForMember(dest => dest.DatosEnvioId, opt => opt.Ignore());
        }
    }
}
