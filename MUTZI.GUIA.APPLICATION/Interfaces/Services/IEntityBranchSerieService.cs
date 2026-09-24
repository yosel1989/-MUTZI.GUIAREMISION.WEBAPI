using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEntityBranchSerieService
    {
        Task<int> CreateAsync(EntityBranchSerieCreateDTO request);
        Task<EntityBranchSerieDTO> UpdateAsync(int entityBranchSerieId, EntityBranchSerieUpdateDTO request);
        Task<EntityBranchSerieDTO?> GetByIdAsync(int entityBranchSerieId);
        Task<PagedResult<EntityBranchSerieDTO>> GetListAsync(int entityBranchId, int pageNumber, int pageSize, string? search);
        Task<ToggleActiveResponseDTO> ToggleActiveAsync(int entityBranchSerieId, bool active);


        Task<List<EntityBranchSerieToSelectDTO>> GetAllByEntityToSelectModal(int entityId, string? search);
    }
}
