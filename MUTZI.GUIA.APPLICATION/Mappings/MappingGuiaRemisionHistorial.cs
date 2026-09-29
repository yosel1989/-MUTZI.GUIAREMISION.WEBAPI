using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionHistorial;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingGuiaRemisionHistorial : AutoMapper.Profile
    {
        public MappingGuiaRemisionHistorial()
        {
            CreateMap<GuiaRemisionHistorial, GuiaRemisionHistorialDTO>()
                .ForMember(dest => dest.AreaNombre, opt => opt.MapFrom(src => src.Area.Name));
            CreateMap<GuiaRemisionHistorial, GuiaRemisionHistorialListDTO>()
                .ForMember(dest => dest.AreaNombre, opt => opt.MapFrom(src => src.Area.Name));
        }
    }
}
