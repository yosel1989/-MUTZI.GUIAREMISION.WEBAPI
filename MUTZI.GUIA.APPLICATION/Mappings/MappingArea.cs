using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingArea : AutoMapper.Profile
    {
        public MappingArea()
        {
            CreateMap<Area, AreaDTO>();
        }
    }
}
