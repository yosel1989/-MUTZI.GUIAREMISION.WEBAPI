using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ISecurityPersonEntityBranchRepository
    {
        Task<bool> CreateAsync(SecurityPersonEntityBranch newData);
        Task<bool> UpdateAsync(SecurityPersonEntityBranch editData);
        Task<(List<SecurityPersonEntityBranch>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);
    }
}
