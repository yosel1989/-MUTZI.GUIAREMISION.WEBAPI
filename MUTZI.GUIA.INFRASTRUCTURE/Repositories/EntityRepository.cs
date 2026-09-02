using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
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
            var query = _context.Entities
                .Include(x => x.DocumentType)
                .Include(x => x.Country)
                .AsQueryable();
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



        public async Task<(int total, List<Entity>)> GetListAsync(int pageNumber, int pageSize, string? search, string? type)
        {
            var query = _context.Entities
                .Where(x => x.Active)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.Name.Contains(search) ||
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.DocumentNumber.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(e => e.Type == type);
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .Select(x => new Entity
                {
                    EntityId = x.EntityId,
                    Type = x.Type,
                    Name = x.Name,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DocumentNumber = x.DocumentNumber,
                    DocumentType = new TipoDocumento
                    {
                        DescripcionCorta = x.DocumentType.DescripcionCorta
                    }
                })
                .ToListAsync();

            return (totalRecords, data);
        }




        /// Validations

        public async Task<bool> HasManyRole(string documentNumber, string role)
        {
            return await _context.Entities
                .Include(x => x.Roles)
                .Where(x => x.DocumentNumber == documentNumber)
                .AnyAsync(x => x.Roles.Any(r => r.Role == role));
        }

        public async Task<bool> HasManyDocumentNumber(string documentNumber)
        {
            return await _context.Entities
                .AnyAsync(x => x.DocumentNumber == documentNumber.Trim());
        }

    }
}
