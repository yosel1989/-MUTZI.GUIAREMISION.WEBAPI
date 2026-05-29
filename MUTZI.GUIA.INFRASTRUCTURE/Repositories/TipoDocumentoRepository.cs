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

        public async Task<IEnumerable<TipoDocumento>> GetListToSelectAsync(string? tipoRegimen)
        {
            var query = _context.TipoDocumento
                .AsQueryable();
            if (!String.IsNullOrWhiteSpace(tipoRegimen))
            { 
                if (tipoRegimen == "natural")
                    query = query.Where(m => m.EsPersonaNatural);
                else if (tipoRegimen == "juridica")
                    query = query.Where(m => m.EsPersonaJuridica);
            }
           
            var data = await query
                .AsNoTracking()
                .ToListAsync();

            return data;
        }

    }
}
