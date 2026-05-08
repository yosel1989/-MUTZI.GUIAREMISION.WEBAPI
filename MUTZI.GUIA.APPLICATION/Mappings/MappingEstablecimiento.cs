using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingEstablecimiento : AutoMapper.Profile
    {
        public MappingEstablecimiento()
        {
            CreateMap<Establecimiento, EstablecimientoDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RucNavigation.Empresa));

            CreateMap<Establecimiento, EstablecimientoListToModalDTO>()
                .ForMember(dest => dest.Entidad, opt => opt.MapFrom(src => src.RucNavigation.Empresa));

            CreateMap<Establecimiento, EstablecimientoListDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito));

            CreateMap<EstablecimientoCreateDTO, Establecimiento>();
            CreateMap<EstablecimientoUpdateDTO, Establecimiento>();

            CreateMap<Establecimiento, EstablecimientoRemitenteDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Ubigeo.Departamento))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Ubigeo.Provincia))
                .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Ubigeo.Distrito))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RucNavigation.Empresa));

            CreateMap<Establecimiento, EstadoActualizadoResponseDTO>()
                .ForMember(dest => dest.Detalle, opt => opt.MapFrom(src => $"Establecimiento con ID {src.EstablecimientoId} ha sido actualizado a estado '{src.Estado}'."));
        }
    }
}
