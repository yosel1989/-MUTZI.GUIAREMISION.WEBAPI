using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IConductorRepository
    {
        Task<List<Conductor>> GetAllAsync();
        Task<List<Conductor>> GetListBySuggestion(string numeroDoc);
        Task<(List<Conductor>, int)> GetListAsync(int pageNumber, int pageSize, string? request);
        Task<Conductor?> GetByIdAsync(int id);
        Task<Conductor?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task CreateAsync(Conductor conductor);
        Task<Conductor> UpdateAsync(Conductor conductor);
        Task DeleteAsync(int id);
    }
}
