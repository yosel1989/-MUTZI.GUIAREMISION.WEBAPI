using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IIntegrationCredentialRepository
    {
        Task<IntegrationCredential> CreateAsync(IntegrationCredential newIntegrationCredential);
        Task<IntegrationCredential> UpdateAsync(IntegrationCredential updateIntegrationCredential);
        Task<IntegrationCredential?> GetByIdAsync(int id);
        Task<IntegrationCredential?> GetByEntityId(int entityId, string provider, bool strict = false);
        Task<(int total, List<IntegrationCredential>)> GetDataTableAsync(int pageNumber, int pageSize, string? search);

        // Validations

        Task<bool> HasManyProvider(int entityId, string provider);
    }
}
