using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemisionEstadoRepository: IGuiaRemisionEstadoRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionEstadoRepository(
            LogisticaContext context
        )
        {
            _context = context;
        }

        public async Task<IEnumerable<GuiaRemisionEstado>> GetAllAsync()
        {
            try
            {
                var data = await _context.GuiaRemisionEstado
                    .OrderBy(m => m.GuiaRemisionEstadoId)
                    .AsNoTracking()
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GuiaRemisionEstado>> GetAllWithPermisosAsync()
        {
            var estados = await _context.GuiaRemisionEstado
                .Select(e => new GuiaRemisionEstado
                {
                    GuiaRemisionEstadoId = e.GuiaRemisionEstadoId,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    EstadosPermitidosDesde = e.EstadosPermitidosDesde.Select(p => new GuiaRemisionEstadoPermiso
                    {
                        GuiaRemisionEstadoIdPermitido = p.GuiaRemisionEstadoIdPermitido
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return estados;
        }
    }
}
