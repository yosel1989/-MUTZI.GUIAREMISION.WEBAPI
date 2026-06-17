using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories.Catalogo
{
    public class BienNormalizadoRepository: IBienNormalizadoRepository
    {
        private readonly LogisticaContext _context;
        public BienNormalizadoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BienNormalizado>> GetAllAsync()
        {
            try
            {
                return await _context.BienNormalizado
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
             
        }

        public async Task<IEnumerable<BienNormalizado>> GetAllToSelectAsync()
        {
            try
            {
                return await _context.BienNormalizado
                    .Select(x => new BienNormalizado
                    {
                        BienNormalizadoId = x.BienNormalizadoId,
                        Descripcion = x.Descripcion,
                        Familia = x.Familia,
                        PartidaArancelaria = x.PartidaArancelaria,
                        CodigoSunat = x.CodigoSunat
                    })
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
