using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ITransportistaRepository
    {
        Task<List<Transportista>> GetAllAsync();
        Task<List<Transportista>> GetListBySuggestion(string numeroDoc);
        Task<(List<Transportista>, int)> GetListAsync(int pageNumber, int pageSize, string? request);
        Task<Transportista?> GetByIdAsync(int id);
        Task CreateAsync(Transportista transporte);
        Task<Transportista> UpdateAsync(Transportista transporte);
        Task DeleteAsync(int id);
    }
}
