using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IEntityBranchSerieRepository
    {
        Task<EntityBranchSerie?> CreateAsync(EntityBranchSerie newEntityBranchSerie);

        Task<EntityBranchSerie?> UpdateAsync(EntityBranchSerie editEntityBranchSerie);

        Task<EntityBranchSerie?> GetByIdAsync(int entityBranchSerieId, bool strict = false);

        Task<bool> HasSerie(int entityBranchId, int? entityBranchSerieId, string serie);

        Task<(List<EntityBranchSerie>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
    }
}
