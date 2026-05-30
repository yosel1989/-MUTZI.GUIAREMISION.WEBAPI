using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class EntidadReguladoraRepository: IEntidadReguladoraRepository
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
