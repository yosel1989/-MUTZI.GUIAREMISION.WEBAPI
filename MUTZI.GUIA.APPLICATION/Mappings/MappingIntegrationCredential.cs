using MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingIntegrationCredential : AutoMapper.Profile
    {
        public MappingIntegrationCredential()
        {

            CreateMap<IntegrationCredential, IntegrationCredentialDTO>()
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.Entity.Name))
                .ForMember(dest => dest.EntityDocumentNumber, opt => opt.MapFrom(src => src.Entity.DocumentNumber));

            CreateMap<IntegrationCredentialCreateDTO, IntegrationCredential>();
            CreateMap<IntegrationCredential, ToggleActiveResponseDTO>();

        }
    }
}
