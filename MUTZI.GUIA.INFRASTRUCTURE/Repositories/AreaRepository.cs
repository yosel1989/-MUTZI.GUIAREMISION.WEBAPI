using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class AreaRepository: IAreaRepository
    {
        private readonly LogisticaContext _context;
        public AreaRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Area>> GetListToSelectAsync()
        {
            return await _context.Area
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetListBySuggestion(string? text)
        {
            try
            {
                var query = _context.Area.Where(c => c.Estado == 1);
                if (!string.IsNullOrEmpty(text))
                {
                    query = query.Where(c =>
                        c.Descripcion.Contains(text)
                    );
                }
                var data = await query.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
