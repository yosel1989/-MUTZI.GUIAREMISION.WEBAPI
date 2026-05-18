using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemisionHistorialRepository: IGuiaRemisionHistorialRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionHistorialRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<GuiaRemisionHistorial>> ObtenerListadoByGuiaAsync(int guiaRemisionId)
        {
            try
            {
                var data = await _context.GuiaRemisionHistorial
                    .Include(m => m.Area)
                    .Where(m => m.GuiaRemisionId == guiaRemisionId)
                    .AsNoTracking()
                    .OrderBy(m => m.GuiaRemisionHistorialId)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GuiaRemisionHistorial> RegistrarAsync(GuiaRemisionHistorial guiaRemisionHistorial)
        {
            try
            {
                _context.GuiaRemisionHistorial.Add(guiaRemisionHistorial);
                await _context.SaveChangesAsync();
                return guiaRemisionHistorial;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
