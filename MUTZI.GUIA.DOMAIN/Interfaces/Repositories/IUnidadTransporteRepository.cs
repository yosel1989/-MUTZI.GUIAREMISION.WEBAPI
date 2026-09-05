using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IUnidadTransporteRepository
    {
        Task<List<TransportUnit>> GetAllAsync();
        Task<List<TransportUnit>> GetListBySuggestion(string numeroDoc);
        Task<(List<TransportUnit>, int)> GetListAsync(int pageNumber, int pageSize, string? request);
        Task<TransportUnit?> GetByIdAsync(int id);
        Task CreateAsync(TransportUnit transporte);
        Task<TransportUnit> UpdateAsync(TransportUnit transporte);
        Task DeleteAsync(int id);
    }
}
