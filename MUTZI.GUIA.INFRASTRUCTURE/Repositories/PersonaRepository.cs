using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class PersonaRepository: IPersonaRepository
    {
        private readonly LogisticaContext _context;
        public PersonaRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetListBySuggestion(string? text)
        {
            try
            {
                var query = _context.Persona.AsQueryable();
                if (!string.IsNullOrEmpty(text))
                {
                    query = query.Where(c =>
                        c.Nombre.Contains(text) ||
                         c.ApellidoPaterno.Contains(text) ||
                          c.ApellidoMaterno.Contains(text)
                    );
                }
                query = query.Where(x => x.Cargo != null);
                var data = await query.AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<Persona?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Persona.Where(x => x.PersonaId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
