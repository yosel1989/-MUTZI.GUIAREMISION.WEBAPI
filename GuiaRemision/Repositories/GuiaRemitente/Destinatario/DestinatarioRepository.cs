
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.GuiaRemitente.Destinatario
{
    public class DestinatarioRepository : IDestinatarioRepository
    {
        private readonly LogisticaContext _context;
        public DestinatarioRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Models.DBMS.Destinatario>> GetAllAsync()
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .ToListAsync();
        }
        public async Task<Models.DBMS.Destinatario?> GetByIdAsync(int id)
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .FirstOrDefaultAsync(d => d.DestinatarioId == id);
        }
        public async Task CreateAsync(Models.DBMS.Destinatario destinatario)
        {
            await _context.Destinatarios.AddAsync(destinatario);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.DBMS.Destinatario destinatario)
        {
            _context.Destinatarios.Update(destinatario);
            destinatario.FechaEdicion = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var destinatario = await _context.Destinatarios.FindAsync(id);
            if (destinatario != null)
            {
                _context.Destinatarios.Remove(destinatario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
