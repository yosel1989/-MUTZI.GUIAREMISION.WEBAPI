
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.GuiaRemitente.Remitente
{
    public class RemitenteRepository : IRemitenteRepository
    {
        private readonly LogisticaContext _context;
        public RemitenteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Models.DBMS.Remitente>> GetAllAsync()
        {
            return await _context.Remitentes
                .Include(r=>r.Ubigeo)
                .ToListAsync();
        }
        public async Task<Models.DBMS.Remitente?> GetByIdAsync(int id)
        {
            return await _context.Remitentes
                .Include(r => r.Ubigeo)
                .FirstOrDefaultAsync(r => r.RemitenteId == id);
        }
        public async Task CreateAsync(Models.DBMS.Remitente remitente)
        {
            await _context.Remitentes.AddAsync(remitente);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.DBMS.Remitente remitente)
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
