using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingEntity : AutoMapper.Profile
    {
        public MappingEntity()
        {
            CreateMap<Entity, EntityDTO>();
            CreateMap<EntityCreateDTO, Entity>();
        }
    }
}
