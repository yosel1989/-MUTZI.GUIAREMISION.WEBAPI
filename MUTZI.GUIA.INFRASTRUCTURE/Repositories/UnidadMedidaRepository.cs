using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class UnidadMedidaRepository: IUnidadMedidaRepository
    {
        private readonly LogisticaContext _context;
        public UnidadMedidaRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<UnidadMedida>> GetListToSelectAsync()
        {
            return await _context.UnidadMedida
                .Where(m => m.Activo)
                .Select(m => new UnidadMedida
                {
                    UnidadMedidaId = m.UnidadMedidaId,
                    CodigoUm = m.CodigoUm,
                    Descripcion = m.Descripcion,
                })
                .ToListAsync();
        }

        public async Task<List<UnidadMedida>> GetListPesoToSelectAsync()
        {
            return await _context.UnidadMedida
                .Where(m => m.Activo && m.EsUnidadPeso)
                .Select(m => new UnidadMedida
                {
                    UnidadMedidaId = m.UnidadMedidaId,
                    CodigoUm = m.CodigoUm,
                    Descripcion = m.Descripcion,
                })
                .ToListAsync();
        }

    }
}
