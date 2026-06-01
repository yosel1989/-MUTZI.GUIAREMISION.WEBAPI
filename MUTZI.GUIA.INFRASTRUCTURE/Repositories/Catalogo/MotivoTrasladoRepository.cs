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
                    .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<MotivoTraslado?> GetByIdAsync(int motivoTrasladoId)
        {
            try
            {
                var data = await _context.MotivoTraslado
                    .Where(m => m.MotivoTrasladoId == motivoTrasladoId)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
