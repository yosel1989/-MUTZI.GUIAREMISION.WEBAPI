
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class IntegrationCredentialRepository: IIntegrationCredentialRepository
    {
        private readonly LogisticaContext _context;
        public IntegrationCredentialRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IntegrationCredential> CreateAsync(IntegrationCredential newIntegrationCredential)
        {
            _context.IntegrationCredentials.Add(newIntegrationCredential);
            await _context.SaveChangesAsync();
            return newIntegrationCredential;
        }

        public async Task<IntegrationCredential> UpdateAsync(IntegrationCredential updateIntegrationCredential)
        {
            _context.IntegrationCredentials.Update(updateIntegrationCredential);
            await _context.SaveChangesAsync();
            return updateIntegrationCredential;
        }

        public async Task<IntegrationCredential?> GetByIdAsync(int id)
        {
            var IntegrationCredential = await _context.IntegrationCredentials.FindAsync(id);
            return IntegrationCredential;
        }

        public async Task<IntegrationCredential?> GetByEntityId(int entityId, string provider)
        {
            var IntegrationCredential = await _context.IntegrationCredentials
                .Where(x => x.EntityId == entityId)
                .Where(x => x.Provider == provider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return IntegrationCredential;
        }

        public async Task<(int total, List<IntegrationCredential>)> GetDataTableAsync(int pageNumber, int pageSize, string? search)
        {
            var query = _context.IntegrationCredentials
                .Include(x => x.Entity)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => 
                    e.Entity.Name.Contains(search) || 
                    e.Entity.FirstName.Contains(search) || 
                    e.Entity.LastName.Contains(search) ||
                    e.Entity.DocumentNumber.Contains(search) ||
                    e.Provider.Contains(search)
                );
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



        /*public async Task<(int total, List<IntegrationCredential>)> GetListAsync(int pageNumber, int pageSize, string? search, string? type)
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
                .Select(x => new IntegrationCredential
                {
                    IntegrationCredentialId = x.IntegrationCredentialId,
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
        */



        /// Validations

        public async Task<bool> HasManyProvider(int entityId, string provider)
        {
            return await _context.IntegrationCredentials
                .Where(x => x.EntityId == entityId)
                .Where(x => x.Provider == provider)
                .AnyAsync();
        }

    }
}
