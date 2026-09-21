using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEntityBranchRepository
    {
        Task<List<EntityBranch>> GetAllByEntityToSelectAsync(int entityId);

        Task<List<EntityBranch>> GetAllByEntityToSelect2Async(int entityId);
        Task<List<EntityBranch>> GetAllByEntityToSelectModal(int entityId);
        Task<List<EntityBranch>> GetListBySuggestion(int entityId, string? search);
        Task<EntityBranch?> GetByIdAsync(int id, bool strict = false, string? tipo = null);
        Task<(List<EntityBranch>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<EntityBranch> UpdateAsync(EntityBranch establecimiento);
        Task CreateAsync(EntityBranch nuevoEstablecimiento);
        Task<bool> GetByCodeSunatAsync(int entityId, string serie);
        Task<GuiaRemision?> GetLastBySerieAsync(string serie, int entityId);
    }
}
