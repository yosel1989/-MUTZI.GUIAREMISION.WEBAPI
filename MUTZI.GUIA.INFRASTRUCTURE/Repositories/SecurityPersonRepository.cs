using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class SecurityPersonRepository : ISecurityPersonRepository
    {
        private readonly LogisticaContext _context;
        public SecurityPersonRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<SecurityPerson?> CreateAsync(SecurityPerson newSecurityPerson)
        {
            await _context.SecurityPersons.AddAsync(newSecurityPerson);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(newSecurityPerson.SecurityPersonId, true);
        }

        public async Task<SecurityPerson?> UpdateAsync(SecurityPerson editSecurityPerson)
        {
            await _context.SaveChangesAsync();

            return await GetByIdAsync(editSecurityPerson.SecurityPersonId, true);
        }

        public async Task<SecurityPerson?> GetByIdAsync(int id, bool strict)
        {
            var role = await _context.SecurityPersons
                .FirstOrDefaultAsync();

            if (strict && role == null)
            {
                throw new KeyNotFoundException($"El personal de seguridad registrado no se encontró.");
            }

            return role;
        }

        public async Task<(List<SecurityPerson>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var query = _context.SecurityPersons.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.DocumentNumber.Contains(search) ||
                    x.Persona.Nombre.Contains(search) ||
                    x.Persona.ApellidoPaterno.Contains(search) ||
                    x.Persona.ApellidoMaterno.Contains(search)
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
