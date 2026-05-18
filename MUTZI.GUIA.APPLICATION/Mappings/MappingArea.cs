using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.Area;

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
