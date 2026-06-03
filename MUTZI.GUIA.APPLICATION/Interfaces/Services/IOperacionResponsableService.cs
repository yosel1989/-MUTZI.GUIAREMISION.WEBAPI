using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.OperacionResponsable;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IOperacionResponsableService
    {
        Task<OperacionResponsableDTO?> GetByIdAsync(int id);
        Task<PagedResult<OperacionResponsableDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
        Task<int> CreateAsync(OperacionResponsableCreateDTO createDTO);
        Task<OperacionResponsableDTO> UpdateAsync(int id, OperacionResponsableUpdateDTO updateDTO);
        Task<List<OperacionResponsableToSelectDTO>> ListToSelectAsync();
    }
}
