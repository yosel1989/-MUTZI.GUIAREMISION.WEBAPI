using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEntityBranchService
    {
        Task<List<EntityBranchListDTO>> GetAllByEntityToSelectAsync(int entityId);
        Task<List<EntityBranchToSelectDTO>> GetAllByEntityToSelect2Async(int entityId);
        Task<List<EntityBranchListToModalDTO>> GetListBySuggestionAsync(int entityId, string? search);
        Task<EntityBranchDTO?> GetByIdAsync(int id);
        Task<PagedResult<EntityBranchDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<ToggleActiveResponseDTO> ToggleActiveAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
        Task<int> CreateAsync(EntityBranchCreateDTO createDTO);

        Task<EntityBranchDTO> UpdateAsync(int id, EntityBranchUpdateDTO updateDTO);
        Task<EstablecimientoRemitenteDTO?> GetByIdForGuiaAsync(int id, string tipoGuia, int entityBranchSerieId);
    }
}
