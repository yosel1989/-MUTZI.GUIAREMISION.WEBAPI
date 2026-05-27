using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingConductor : AutoMapper.Profile
    {
        public MappingConductor()
        {
            CreateMap<ConductorGuiaUpdateDTO, GuiaRemisionConductor>();
            CreateMap<Conductor, ConductorDTO>();
            CreateMap<Conductor, ConductorListDTO>();
            CreateMap<ConductorCreateDTO, Conductor>()
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres.ToUpper()))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos.ToUpper()))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo.ToUpper()));
            CreateMap<ConductorUpdateDTO, Conductor>();
            CreateMap<Conductor, EstadoActualizadoResponseDTO>();
        }
    }
}
