using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonReasonForTransfer;


namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface ISecurityPersonReasonForTransferService
    {
        Task<bool> CreateAsync(SecurityPersonReasonForTransferCreateDTO request);
        Task<bool> DeleteAsync(SecurityPersonReasonForTransferDeleteDTO request);
        Task<PagedResult<SecurityPersonReasonForTransferDTO>> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);
    }
}
