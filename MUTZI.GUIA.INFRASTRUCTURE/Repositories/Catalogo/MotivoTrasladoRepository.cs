using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class MotivoTrasladoRepository: IMotivoTrasladoRepository
    {
        private readonly LogisticaContext _context;
        public MotivoTrasladoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<MotivoTraslado>> GetListToSelectAsync()
        {
            try
            {
                var data  = await _context.MotivoTraslado
                    .Where(x => x.Active)
                    .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<MotivoTraslado?> GetByIdAsync(int motivoTrasladoId, bool strict)
        {
            try
            {
                var data = await _context.MotivoTraslado
                    .Where(m => m.MotivoTrasladoId == motivoTrasladoId)
                    .FirstOrDefaultAsync();

                if (strict && data is null) {
                    throw new Exception("No existe el motivo de traslado en los registros.");
                }

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
