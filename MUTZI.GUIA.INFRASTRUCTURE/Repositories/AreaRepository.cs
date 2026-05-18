using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
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

    }
}
