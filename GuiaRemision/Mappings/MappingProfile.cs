using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.GuiaRemitente.Conductor;
using GuiaRemision.DTOs.GuiaRemitente.DatosDestino;
using GuiaRemision.DTOs.GuiaRemitente.DatosEnvio;
using GuiaRemision.DTOs.GuiaRemitente.Destino;
using GuiaRemision.DTOs.GuiaRemitente.Origen;
using GuiaRemision.DTOs.GuiaRemitente.Productos;
using GuiaRemision.DTOs.GuiaRemitente.Proveedor;
using GuiaRemision.DTOs.GuiaRemitente.Remitente;
using GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte;
using GuiaRemision.DTOs.GuiaTransportista;
using GuiaRemision.DTOs.GuiaTransportista.DatosEnvio;
using GuiaRemision.DTOs.GuiaTransportista.RemiteTransportista;
using GuiaRemision.DTOs.VentaConfig;
using GuiaRemision.Models.DBMS;
using System.Globalization;

namespace GuiaRemision.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<VentaConfig, VentaConfigAccessDTO>();
            CreateMap<Conductor, ConductorDTO>();
            CreateMap<DTOs.GuiaRemitente.Conductor.ConductorCreateDTO, Conductor>();
            CreateMap<ConductorUpdateDTO, Conductor>();

            //Guias de remisión
            CreateMap<Models.DBMS.GuiaRemision, GuiaRemitenteDTO>()
                .ForMember(dest => dest.SerieCorrelativo, opt => opt.MapFrom(src => src.NumeroGuia))
                .ForMember(dest=>dest.Fecha, opt=>opt.MapFrom(src=>src.Fecha.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Remitente, opt => opt.MapFrom(src => src.Remitente))
                .ForMember(dest => dest.Destinatario, opt => opt.MapFrom(src => src.Destinatario))
                .ForMember(dest => dest.DatosEnvio, opt => opt.MapFrom(src => src.DatosEnvios.FirstOrDefault()))
                //Mapear conductor y transporte dentro de DatosEnvio
                //.ForMember(dest=>dest.DatosEnvio.Conductor, opt=>opt.MapFrom(src=>src.DatosEnvios.con))

                .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Origens.FirstOrDefault()))
                .ForMember(dest => dest.Destino, opt => opt.MapFrom(src => src.Destinos))
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.GuiaDetalles))
                ;
            CreateMap<GuiaRemitenteCreateDTO, Models.DBMS.GuiaRemision>()
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                    .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                    .ForMember(dest => dest.Destinatario, opt => opt.Ignore())
                    .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                    ;
            //Guia de remisión por ticket
            CreateMap<Models.DBMS.GuiaRemision, GuiaPorTicketDTO>()
                .ForMember(dest => dest.Ruc, opt => opt.MapFrom(src => src.Remitente.Ruc))
                ;

            //Guias de transportista
            CreateMap<Models.DBMS.GuiaRemision, GuiaTransportistaDTO>()
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
            CreateMap<GuiaTransportistaCreateDTO, Models.DBMS.GuiaRemision>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                .ForMember(dest => dest.RemiteTransportista, opt => opt.Ignore())
                .ForMember(dest => dest.Destinatario, opt => opt.Ignore());

            CreateMap<GuiaRemitenteUpdateDTO, Models.DBMS.GuiaRemision>();
            //Datos de envío
            CreateMap<Models.DBMS.DatosEnvio, DatosEnvioDTO>()
                .ForMember(dest=>dest.Conductor, opt=>opt.MapFrom(src=>src.Conductor))
                .ForMember(dest=>dest.UnidadTransporte, opt=>opt.MapFrom(src=>src.Transporte))
                .ForMember(dest=>dest.FechaEnvio,opt=>opt.MapFrom(src=>src.FechaEnvio.ToString("yyyy-MM-dd")))
                ;

            CreateMap<DatosEnvioCreateDTO, Models.DBMS.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest => dest.Conductor, opt => opt.Ignore())
                .ForMember(dest => dest.Transporte, opt => opt.Ignore());
            //Datos de envío para guía transportista
            CreateMap<Models.DBMS.DatosEnvio, DatosEnvioGT_DTO>()
                .ForMember(dest => dest.Conductor, opt => opt.MapFrom(src => src.Conductor))
                .ForMember(dest => dest.UnidadTransporte, opt => opt.MapFrom(src => src.Transporte))
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => src.FechaEnvio.ToString("yyyy-MM-dd")))
                ;
            CreateMap<DatosEnvioGTCreateDTO, Models.DBMS.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest => dest.Conductor, opt => opt.Ignore())
                .ForMember(dest => dest.Transporte, opt => opt.Ignore());
            //Remitente
            CreateMap<Models.DBMS.Remitente, RemitenteDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<RemitenteCreateDTO, Remitente>();

            //Remite transportista
            CreateMap<Models.DBMS.RemiteTransportista, RemiteTransportistaDTO>()
                .ForMember(dest=>dest.Distrito, opt=>opt.MapFrom(src=>src.Ubigeo.Distrito))
                .ForMember(dest=>dest.Provincia, opt=>opt.MapFrom(src=>src.Ubigeo.Provincia))
                .ForMember(dest=>dest.Departamento, opt=>opt.MapFrom(src=>src.Ubigeo.Departamento))
                ;
            CreateMap<RemiteTransportistaCreateDTO, Models.DBMS.RemiteTransportista>();

            //Destinatario
            CreateMap<Models.DBMS.Destinatario, DestinatarioDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<DestinatarioCreateDTO, Models.DBMS.Destinatario>();
            //Proveedor
            CreateMap<Models.DBMS.Proveedor, ProveedorDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<ProveedorCreateDTO, Models.DBMS.Proveedor>();
            //Productos
            CreateMap<Models.DBMS.GuiaDetalle, ProductosDTO>()
                .ForMember(dest => dest.DescripcionUM, opt => opt.MapFrom(src => src.UnidadMedida.Descripcion));
            CreateMap<ProductosCreateDTO, Models.DBMS.GuiaDetalle>();
            //Conductor
            CreateMap<Models.DBMS.Conductor, ConductorDTO>();
            CreateMap<ConductorCreateDTO, Models.DBMS.Conductor>();
            //Unidad de transporte
            CreateMap<Models.DBMS.Transporte, UnidadTransporteDTO>();
            CreateMap<UnidadTransporteCreateDTO, Models.DBMS.Transporte>();
            //Ubicación origen
            CreateMap<Models.DBMS.Origen, OrigenDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<OrigenCreateDTO, Models.DBMS.Origen>();
            //Ubicación destino
            CreateMap<Models.DBMS.Destino, DestinoDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<DestinoCreateDTO, Models.DBMS.Destino>();

            //
        }
    }
}
