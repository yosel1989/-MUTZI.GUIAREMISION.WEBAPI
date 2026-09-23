using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch;


namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface ISecurityPersonEntityBranchService
    {
        Task<bool> CreateAsync(SecurityPersonEntityBranchCreateDTO request);
        Task<bool> DeleteAsync(SecurityPersonEntityBranchDeleteDTO request);
        Task<PagedResult<SecurityPersonEntityBranchDTO>> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);
    }
}
