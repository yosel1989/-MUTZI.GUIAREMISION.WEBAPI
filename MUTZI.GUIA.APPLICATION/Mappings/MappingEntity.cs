using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.APPLICATION.DTOs.PersonaEmpresaInfo;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Models;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingEntity : AutoMapper.Profile
    {
        public MappingEntity()
        {
            CreateMap<Entity, EntityDTO>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType.DescripcionCorta))
                ;
            CreateMap<EntityCreateDTO, Entity>();

            CreateMap<PersonInfo, PersonInfoDTO>();
            CreateMap<CompanyInfo, CompanyInfoDTO>();

        }
    }
}
