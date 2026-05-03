using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;

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

            CreateMap<Establecimiento, EstablecimientoListToModalDTO>();

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
        }
    }
}
