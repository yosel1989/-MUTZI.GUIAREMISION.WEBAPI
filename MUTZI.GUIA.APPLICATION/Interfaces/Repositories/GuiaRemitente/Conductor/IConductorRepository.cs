using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Conductor
{
    public interface IConductorRepository
    {
        Task<List<DOMAIN.Entities.Conductor>> GetAllAsync();
        Task<List<DOMAIN.Entities.Conductor>> GetListBySuggestion(string numeroDoc);
        Task<(List<DOMAIN.Entities.Conductor>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO? request);
        Task<DOMAIN.Entities.Conductor?> GetByIdAsync(int id);
        Task<DOMAIN.Entities.Conductor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(DOMAIN.Entities.Conductor conductor);
        Task UpdateAsync(DOMAIN.Entities.Conductor conductor);
        Task DeleteAsync(int id);
    }
}
