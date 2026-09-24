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

        public async Task<IEnumerable<Persona>> GetListBySuggestion(
            string? text,
            string? cargo)
        {
            try
            {
                var query = _context.Persona
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    query = query.Where(c =>
                        (c.Nombre != null && c.Nombre.Contains(text)) ||
                        (c.NumeroDocumento != null && c.NumeroDocumento.Contains(text)) ||
                        (c.ApellidoPaterno != null && c.ApellidoPaterno.Contains(text)) ||
                        (c.Cargo != null && c.Cargo.Contains(text)) ||
                        (c.ApellidoMaterno != null && c.ApellidoMaterno.Contains(text))
                    );
                }

                if (!string.IsNullOrWhiteSpace(cargo))
                {
                    query = query.Where(c =>
                        c.Cargo != null && c.Cargo.Contains(cargo)
                    );
                }

                var data = await query
                    .AsNoTracking()
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
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
