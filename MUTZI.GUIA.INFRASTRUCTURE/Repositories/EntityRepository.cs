using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class EntityRepository: IEntityRepository
    {
        private readonly LogisticaContext _context;
        public EntityRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<Entity> CreateAsync(Entity newEntity)
        {
            _context.Entities.Add(newEntity);
            await _context.SaveChangesAsync();
            return newEntity;
        }

        public async Task<Entity> UpdateAsync(Entity updateEntity)
        {
            _context.Entities.Update(updateEntity);
            await _context.SaveChangesAsync();
            return updateEntity;
        }

        public async Task<Entity?> GetByIdAsync(int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            return entity;
        }

        public async Task<Entity?> GetByDocumentNumber(string documentNumber)
        {
            var entity = await _context.Entities
                .Include(x => x.Roles)
                .Where(x => x.DocumentNumber == documentNumber)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<(int total, List<Entity>)> GetDataTableAsync(int pageNumber, int pageSize, string? search, string role)
        {
            var query = _context.Entities.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => 
                    e.Name.Contains(search) || 
                    e.FirstName.Contains(search) || 
                    e.LastName.Contains(search) ||
                    e.DocumentType.DescripcionCorta.Contains(search) ||
                    e.DocumentNumber.Contains(search) ||
                    e.Country.Name.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(e => e.Roles.Any(x => x.Role == role));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (totalRecords, data);
        }
    }
}
