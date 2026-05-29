using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class EntidadReguladoraRepository
    {
        private readonly LogisticaContext _context;
        public EntidadReguladoraRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EntidadReguladora>> GetAllAsync()
        {
            try
            {
                return await _context.EntidadReguladora.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
             
        }

    }
}
