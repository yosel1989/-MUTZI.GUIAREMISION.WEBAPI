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
            try
            {
                await _context.SecurityPersonEntityBranchs.AddAsync(newData);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("security_person__entity_branch.PRIMARY"))
                {
                    throw new Exception("El personal ya tiene el establecimiento asignado.");
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(SecurityPersonEntityBranch model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(List<SecurityPersonEntityBranch>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var query = _context.SecurityPersonEntityBranchs
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
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SecurityPersonEntityBranch
                {
                    SecurityPersonId = x.SecurityPersonId,
                    EntityBranchId = x.EntityBranchId,
                    CreatedAt = x.CreatedAt,
                    CreatedAtUser = x.CreatedAtUser,
                    CreatedAtUserName = x.CreatedAtUserName,
                    EntityBranch = new EntityBranch
                    {
                        Address = x.EntityBranch.Address,
                        Description = x.EntityBranch.Description,
                        CodeSunat = x.EntityBranch.CodeSunat,
                        Entity = new Entity
                        {
                            Name = x.EntityBranch.Entity.Name,
                            DocumentNumber = x.EntityBranch.Entity.DocumentNumber
                        }
                    }
                })
                .ToListAsync();
            return (data, totalRecords);

        }

    }
}
