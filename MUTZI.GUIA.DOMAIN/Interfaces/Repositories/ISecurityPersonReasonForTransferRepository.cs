using MUTZI.GUIA.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ISecurityPersonReasonForTransferRepository
    {
        Task<bool> CreateAsync(SecurityPersonReasonForTransfer newData);
        Task<bool> DeleteAsync(SecurityPersonReasonForTransfer model);
        Task<(List<SecurityPersonReasonForTransfer>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);
        Task<List<int>> GetAllReasonForTransferIdsByAssigned(int employeeId);
        Task<List<SecurityPersonReasonForTransfer>> GetListByAssignedAsync(int employeeId);
    }
}
