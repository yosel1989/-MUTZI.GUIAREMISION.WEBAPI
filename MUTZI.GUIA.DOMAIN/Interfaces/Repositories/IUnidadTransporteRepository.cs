using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IUnidadTransporteRepository
    {
        Task<List<UnidadTransporte>> GetAllAsync();
        Task<List<UnidadTransporte>> GetListBySuggestion(string numeroDoc);
        Task<(List<UnidadTransporte>, int)> GetListAsync(int pageNumber, int pageSize, string? request);
        Task<UnidadTransporte?> GetByIdAsync(int id);
        Task CreateAsync(UnidadTransporte transporte);
        Task<UnidadTransporte> UpdateAsync(UnidadTransporte transporte);
        Task DeleteAsync(int id);
    }
}
