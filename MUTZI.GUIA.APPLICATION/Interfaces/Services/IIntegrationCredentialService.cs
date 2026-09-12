using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IIntegrationCredentialService
    {
        Task<IntegrationCredentialDTO> CreateAsync(IntegrationCredentialCreateDTO newIntegrationCredential);

        Task<IntegrationCredentialDTO> UpdateAsync(int id, IntegrationCredentialUpdateDTO updateIntegrationCredential);

        Task<IntegrationCredentialDTO> GetByIdAsync(int id);

        Task<IntegrationCredentialDTO> GetByEntityAsync(int entityId, string provider);

        Task<PagedResult<IntegrationCredentialDTO>> GetDataTableAsync(int pageNumber, int pageSize, string? search);

        Task<ToggleActiveResponseDTO> ToggleActiveAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);

        List<object> GetProviderToSelect();

        Task DeleteAsync(int id);



        Task<TokenResponseDTO> GetTokenAsync(int entityId, string provider);

        Task<string> ObtenerGuidDesdeTicketAsync(string numeroGuia, string token);

        Task<byte[]> ObtenerDocumentoAsync(string tipo, string guid, string token);
    }
}
