using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class TipoDocumentoRepository: ITipoDocumentoRepository
    {
        private readonly LogisticaContext _context;
        public TipoDocumentoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoDocumento>> GetListToSelectAsync()
        {
            return await _context.TipoDocumento
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
