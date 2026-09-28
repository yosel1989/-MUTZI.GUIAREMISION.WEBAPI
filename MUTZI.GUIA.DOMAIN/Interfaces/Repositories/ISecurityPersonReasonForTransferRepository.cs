using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ISecurityPersonReasonForTransferRepository
    {
        Task<bool> CreateAsync(SecurityPersonReasonForTransfer newData);
        Task<bool> DeleteAsync(SecurityPersonReasonForTransfer model);
        Task<(List<SecurityPersonReasonForTransfer>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);
        Task<List<int>> GetAllReasonForTransferIdsByAssigned(int employeeId);
        Task<List<SecurityPersonReasonForTransfer>> GetListByAssignedAsync(int employeeId);
        Task<List<int>> GetAllReasonIdsByAssigned(int employeeId);

        // Validators

        Task<bool> HasAssigned(int employeeId, int reasonForTransferId);
    }
}
