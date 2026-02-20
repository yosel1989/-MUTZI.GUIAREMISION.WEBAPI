
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Remitente;
using Microsoft.EntityFrameworkCore;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class RemitenteRepository : IRemitenteRepository
    {
        private readonly LogisticaContext _context;
        public RemitenteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Remitente>> GetAllAsync()
        {
            return await _context.Remitentes
                .Include(r=>r.Ubigeo)
                .Include(e=>e.RucNavigation)
                .ToListAsync();
        }
        public async Task<DOMAIN.Entities.GuiaRemision?> GetLastBySerieAsync(string serie)
        {
            if (string.IsNullOrWhiteSpace(serie))
                return null;

            return await _context.GuiaRemisions
                .Where(g => g.Serie == serie)
                .OrderByDescending(g => g.Correlativo)
                .FirstOrDefaultAsync();
        }
        public async Task<DOMAIN.Entities.Remitente?> GetByIdAsync(int id, string tipoGuia)
        {
            return await _context.Remitentes
                .Include(r => r.Ubigeo)
                .Include(e => e.RucNavigation)
                .FirstOrDefaultAsync(r => r.RemitenteId == id);
        }
        public async Task CreateAsync(DOMAIN.Entities.Remitente remitente)
        {
            await _context.Remitentes.AddAsync(remitente);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Remitente remitente)
        {
            _context.Remitentes.Update(remitente);
            remitente.FechaEdicion = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var remitente = await _context.Remitentes.FindAsync(id);
            if (remitente != null)
            {
                _context.Remitentes.Remove(remitente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
