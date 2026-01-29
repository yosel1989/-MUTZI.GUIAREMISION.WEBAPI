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

namespace MUTZI.GUIA.APPLICATION.Mappings
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
            CreateMap<DOMAIN.Entities.GuiaRemision, GuiaRemitenteDTO>()
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
            CreateMap<GuiaRemitenteCreateDTO, DOMAIN.Entities.GuiaRemision>()
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Fecha) ? (DateOnly?)null : DateOnly.Parse(src.Fecha)
            ))
                    .ForMember(dest => dest.Remitente, opt => opt.Ignore())
                    .ForMember(dest => dest.Destinatario, opt => opt.Ignore())
                    .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                    ;
            //Guia de remisión por ticket
            CreateMap<DOMAIN.Entities.GuiaRemision, GuiaPorTicketDTO>()
                .ForMember(dest => dest.Ruc, opt => opt.MapFrom(src => src.Remitente.Ruc))
                ;

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

            CreateMap<GuiaRemitenteUpdateDTO, DOMAIN.Entities.GuiaRemision>();
            //Datos de envío
            CreateMap<DOMAIN.Entities.DatosEnvio, DatosEnvioDTO>()
                .ForMember(dest=>dest.Conductor, opt=>opt.MapFrom(src=>src.Conductor))
                .ForMember(dest=>dest.UnidadTransporte, opt=>opt.MapFrom(src=>src.Transporte))
                .ForMember(dest=>dest.FechaEnvio,opt=>opt.MapFrom(src=>src.FechaEnvio.ToString("yyyy-MM-dd")))
                ;

            CreateMap<DatosEnvioCreateDTO, DOMAIN.Entities.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest => dest.Conductor, opt => opt.Ignore())
                .ForMember(dest => dest.Transporte, opt => opt.Ignore());
            //Datos de envío para guía transportista
            CreateMap<DOMAIN.Entities.DatosEnvio, DatosEnvioGT_DTO>()
                .ForMember(dest => dest.Conductor, opt => opt.MapFrom(src => src.Conductor))
                .ForMember(dest => dest.UnidadTransporte, opt => opt.MapFrom(src => src.Transporte))
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => src.FechaEnvio.ToString("yyyy-MM-dd")))
                ;
            CreateMap<DatosEnvioGTCreateDTO, DOMAIN.Entities.DatosEnvio>()
                .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FechaEnvio) ? (DateOnly?)null : DateOnly.Parse(src.FechaEnvio)))
                .ForMember(dest => dest.Conductor, opt => opt.Ignore())
                .ForMember(dest => dest.Transporte, opt => opt.Ignore());
            //Remitente
            CreateMap<DOMAIN.Entities.Remitente, RemitenteDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<RemitenteCreateDTO, Remitente>();
            CreateMap<RemitenteUpdateDTO, Remitente>();

            //Remite transportista
            CreateMap<DOMAIN.Entities.RemiteTransportista, RemiteTransportistaDTO>()
                .ForMember(dest=>dest.Distrito, opt=>opt.MapFrom(src=>src.Ubigeo.Distrito))
                .ForMember(dest=>dest.Provincia, opt=>opt.MapFrom(src=>src.Ubigeo.Provincia))
                .ForMember(dest=>dest.Departamento, opt=>opt.MapFrom(src=>src.Ubigeo.Departamento))
                ;
            CreateMap<RemiteTransportistaCreateDTO, DOMAIN.Entities.RemiteTransportista>();

            //Destinatario
            CreateMap<DOMAIN.Entities.Destinatario, DestinatarioDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<DestinatarioCreateDTO, DOMAIN.Entities.Destinatario>();
            CreateMap<DestinatarioUpdateDTO, DOMAIN.Entities.Destinatario>();
            //Proveedor
            CreateMap<DOMAIN.Entities.Proveedor, ProveedorDTO>()
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                ;
            CreateMap<ProveedorCreateDTO, DOMAIN.Entities.Proveedor>();
            CreateMap<ProveedorUpdateDTO, DOMAIN.Entities.Proveedor>();
            //Productos
            CreateMap<DOMAIN.Entities.GuiaDetalle, ProductosDTO>()
                .ForMember(dest => dest.DescripcionUM, opt => opt.MapFrom(src => src.UnidadMedida.Descripcion));
            CreateMap<ProductosCreateDTO, DOMAIN.Entities.GuiaDetalle>();
            //Conductor
            CreateMap<DOMAIN.Entities.Conductor, ConductorDTO>();
            CreateMap<ConductorCreateDTO, DOMAIN.Entities.Conductor>();
            CreateMap<ConductorUpdateDTO, DOMAIN.Entities.Conductor>();

            //Unidad de transporte
            CreateMap<DOMAIN.Entities.Transporte, UnidadTransporteDTO>();
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
        }
    }
}
