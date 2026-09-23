using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class SecurityPersonEntityBranchSerieRepository : ISecurityPersonEntityBranchSerieRepository
    {
        private readonly LogisticaContext _context;
        public SecurityPersonEntityBranchSerieRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(SecurityPersonEntityBranchSerie newData)
        {
            await _context.SecurityPersonEntityBranchSeries.AddAsync(newData);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(SecurityPersonEntityBranchSerie model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(List<SecurityPersonEntityBranchSerie>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var query = _context.SecurityPersonEntityBranchSeries
                .Where(x => x.SecurityPersonId == securityPersonId)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.EntityBranchSerie.Serie.Contains(search)
                );
            }
            var totalRecords = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SecurityPersonEntityBranchSerie
                {
                    SecurityPersonId = x.SecurityPersonId,
                    EntityBranchSerieId = x.EntityBranchSerieId,
                    CreatedAt = x.CreatedAt,
                    CreatedAtUser = x.CreatedAtUser,
                    CreatedAtUserName = x.CreatedAtUserName,
                    EntityBranchSerie = new EntityBranchSerie
                    {
                        Serie = x.EntityBranchSerie.Serie,
                        EntityBranch = new EntityBranch
                        {
                            Address = x.EntityBranchSerie.EntityBranch.Address,
                            Description = x.EntityBranchSerie.EntityBranch.Description,
                            CodeSunat = x.EntityBranchSerie.EntityBranch.CodeSunat,
                            Entity = new Entity
                            {
                                DocumentNumber = x.EntityBranchSerie.EntityBranch.Entity.DocumentNumber,
                                Name = x.EntityBranchSerie.EntityBranch.Entity.Name
                            }
                        }
                    }
                })
                .ToListAsync();
            return (data, totalRecords);

        }


        public async Task<List<int>> GetAllEntityIdsByAssigned(int employeeId)
        {
            var data = _context.SecurityPersonEntityBranchSeries
                .Where(x => x.SecurityPerson.PersonId == employeeId)
                .Select(x => x.EntityBranchSerie.EntityBranch.EntityId)
                .Distinct()
                .ToList();

            return data;
        }

    }
}
