using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IEstablecimientoService
    {
        Task<List<EntityBranchListDTO>> GetAllByRucToSelectAsync(string ruc);
        Task<List<EntityBranchToSelectDTO>> GetAllByRucToSelect2Async(string ruc);
        Task<List<EntityBranchListToModalDTO>> GetListBySuggestionAsync(string ruc, string? search);
        Task<EntityBranchDTO?> GetByIdAsync(int id);
        Task<PagedResult<EntityBranchDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<EstadoActualizadoResponseDTO> CambiarEstadoAsync(int id, EstadoActualizadoRequestDTO infoRequestDto);
        Task<int> CreateAsync(EntityBranchCreateDTO createDTO);

        Task<EntityBranchDTO> UpdateAsync(int id, EntityBranchUpdateDTO updateDTO);
        Task<EstablecimientoRemitenteDTO?> GetByIdForGuiaAsync(int id, string tipoGuia);
    }
}
