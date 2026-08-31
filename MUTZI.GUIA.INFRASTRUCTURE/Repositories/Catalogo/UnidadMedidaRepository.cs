using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class UnidadMedidaRepository: IUnidadMedidaRepository
    {
        private readonly LogisticaContext _context;
        public UnidadMedidaRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnitMeasurement>> GetListPesoToSelectAsync(string? tipo)
        {
            var query = _context.UnidadMedida.AsQueryable();

            switch (tipo)
            {
                case "peso":
                    query = query.Where(u => u.EsUnidadPeso);
                    break;
                case "volumen":
                    query = query.Where(u => u.EsUnidadVolumen);
                    break;
                case "longitud":
                    query = query.Where(u => u.EsUnidadLongitud);
                    break;
                case "conteo":
                    query = query.Where(u => u.EsUnidadConteo);
                    break;
                case "guia-remision":
                    query = query.Where(u => u.EsGuiaRemision);
                    break;
                case null:
                case "":
                    break;
                default:
                    break;
            }
                
            return await query.AsNoTracking().ToListAsync();
        }

    }
}
