using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ISecurityPersonEntityBranchSerieRepository
    {
        Task<bool> CreateAsync(SecurityPersonEntityBranchSerie newData);
        Task<bool> DeleteAsync(SecurityPersonEntityBranchSerie model);
        Task<(List<SecurityPersonEntityBranchSerie>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search);

        Task<List<int>> GetAllEntityIdsByAssigned(int employeeId);
    }
}
