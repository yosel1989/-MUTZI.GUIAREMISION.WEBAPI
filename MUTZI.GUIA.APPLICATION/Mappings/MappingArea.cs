using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingArea : AutoMapper.Profile
    {
        public MappingArea()
        {
            CreateMap<Area, AreaDTO>();
            CreateMap<Area, ToggleActiveResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AreaId))
                ;
            CreateMap<AreaCreateDTO, Area>();
            CreateMap<Area, AreaToSelectDTO>();
        }
    }
}
