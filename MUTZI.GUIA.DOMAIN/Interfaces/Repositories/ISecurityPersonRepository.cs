using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface ISecurityPersonRepository
    {
        Task<SecurityPerson?> CreateAsync(SecurityPerson newSecurityPerson);
        Task<SecurityPerson?> UpdateAsync(SecurityPerson editSecurityPerson);
        Task<SecurityPerson?> GetByIdAsync(int id, bool strict = false);
        Task<(List<SecurityPerson>, int)> GetListAsync(int pageNumber, int pageSize, string? search);
    }
}
