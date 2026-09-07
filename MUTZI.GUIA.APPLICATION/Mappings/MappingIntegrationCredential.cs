using MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Mappings
{
    public class MappingIntegrationCredential : AutoMapper.Profile
    {
        public MappingIntegrationCredential()
        {

            CreateMap<IntegrationCredential, IntegrationCredentialDTO>();
            CreateMap<IntegrationCredentialCreateDTO, IntegrationCredential>();

        }
    }
}
