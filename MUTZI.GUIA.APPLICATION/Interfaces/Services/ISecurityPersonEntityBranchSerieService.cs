using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie;


namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface ISecurityPersonEntityBranchSerieService
    {
        Task<bool> CreateAsync(SecurityPersonEntityBranchSerieCreateDTO request);
        Task<bool> DeleteAsync(SecurityPersonEntityBranchSerieDeleteDTO request);
        Task<PagedResult<SecurityPersonEntityBranchSerieDTO>> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);
    }
}
