using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEstablecimientoRepository
    {
        Task<List<EntityBranch>> GetAllByRucToSelectAsync(string ruc);

        Task<List<EntityBranch>> GetAllByRucToSelect2Async(string ruc);
        Task<List<EntityBranch>> GetAllByRucToSelectModal(string ruc);
        Task<List<EntityBranch>> GetListBySuggestion(string ruc, string? search);
        Task<EntityBranch?> GetByIdAsync(int id, bool strict = false, string? tipo = null);
        Task<(List<EntityBranch>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<EntityBranch> UpdateAsync(EntityBranch establecimiento);
        Task CreateAsync(EntityBranch nuevoEstablecimiento);
        Task<bool> GetBySerieAsync(string ruc, string serie);
        Task<GuiaRemision?> GetLastBySerieAsync(string serie, string ruc);
    }
}
