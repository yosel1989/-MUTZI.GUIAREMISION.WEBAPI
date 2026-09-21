using MUTZI.GUIA.APPLICATION.DTOs.DataTable;

using MUTZI.GUIA.APPLICATION.DTOs.SecurityPerson;


namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface ISecurityPersonService
    {
        Task<int> CreateAsync(SecurityPersonCreateDTO request);
        Task<SecurityPersonDTO> UpdateAsync(int id, SecurityPersonUpdateDTO request);
        Task<SecurityPersonDTO?> GetByIdAsync(int id);
        Task<PagedResult<SecurityPersonDTO>> GetListAsync(int pageNumber, int pageSize, string? search);
    }
}
