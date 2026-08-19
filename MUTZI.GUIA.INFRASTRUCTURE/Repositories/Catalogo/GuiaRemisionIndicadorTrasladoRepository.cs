using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class GuiaRemisionIndicadorTrasladoRepository: IGuiaRemisionIndicadorTrasladoRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionIndicadorTrasladoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuiaRemisionIndicadorTraslado>> GetListToSelectAsync()
        {
            var data = _context.GuiaRemisionIndicadorTraslado
                .AsNoTracking()
                .ToListAsync();

            return await data;
        }

    }
}
