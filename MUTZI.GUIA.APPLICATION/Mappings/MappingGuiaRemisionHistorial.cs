using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionHistorial;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingGuiaRemisionHistorial : AutoMapper.Profile
    {
        public MappingGuiaRemisionHistorial()
        {
            CreateMap<GuiaRemisionHistorial, GuiaRemisionHistorialDTO>();
            CreateMap<GuiaRemisionHistorial, GuiaRemisionHistorialListDTO>();
        }
    }
}
