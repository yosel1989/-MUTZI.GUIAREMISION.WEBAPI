using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.APPLICATION.DTOs.EntityRole;
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
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Code))
                ;
            CreateMap<Entity, EntityByRoleDTO>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType.DescripcionCorta))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Code))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Roles.FirstOrDefault().Active ) )
                ;
            CreateMap<EntityCreateDTO, Entity>();
            CreateMap<Entity, EntityListDTO>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType.DescripcionCorta))
                ;

            CreateMap<EntityRole, EntityRoleDTO>();

            CreateMap<PersonInfo, PersonInfoDTO>();
            CreateMap<CompanyInfo, CompanyInfoDTO>();
        }
    }
}
