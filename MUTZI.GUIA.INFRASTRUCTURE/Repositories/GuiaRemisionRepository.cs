using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemisionRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionRepository(LogisticaContext context)
        {
            _context = context;
        }

    }
}
