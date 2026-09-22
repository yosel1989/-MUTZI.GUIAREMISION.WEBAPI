using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class SecurityPersonEntityBranchRepository : ISecurityPersonEntityBranchRepository
    {
        private readonly LogisticaContext _context;
        public SecurityPersonEntityBranchRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(SecurityPersonEntityBranch newData)
        {
            await _context.SecurityPersonEntityBranchs.AddAsync(newData);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(SecurityPersonEntityBranch editData)
        {
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(List<SecurityPersonEntityBranch>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var query = _context.SecurityPersonEntityBranchs
                .Include(x => x.EntityBranch)
                .Where(x => x.SecurityPersonId == securityPersonId)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.EntityBranch.Address.Contains(search) ||
                    x.EntityBranch.Description.Contains(search)
                );
            }
            var totalRecords = await query.CountAsync();
            var securityPersons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (securityPersons, totalRecords);

        }

    }
}
