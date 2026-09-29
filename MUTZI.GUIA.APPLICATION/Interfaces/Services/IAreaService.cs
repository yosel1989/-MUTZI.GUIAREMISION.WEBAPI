using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IAreaService
    {
        Task<List<AreaToSelectDTO>> GetListToSelectAsync(int? level);
        Task<int> CreateAsync(AreaCreateDTO request);
        Task<AreaDTO> UpdateAsync(AreaUpdateDTO request);
        Task<AreaDTO?> GetByIdAsync(int areaId, bool strict = false);
        Task<PagedResult<AreaDTO>> GetToTableAsync(int pageNumber, int pageSize, string? search);
        Task DeleteAsync(int id);
        Task<ToggleActiveResponseDTO> ToggleActiveAsync(int id, bool active);
    }
}
