
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class DestinatarioRepository : IDestinatarioRepository
    {
        private readonly LogisticaContext _context;
        public DestinatarioRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Destinatario>> GetAllAsync()
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .ToListAsync();
        }
        public async Task<DOMAIN.Entities.Destinatario?> GetByIdAsync(int id)
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .FirstOrDefaultAsync(d => d.DestinatarioId == id);
        }
        public async Task CreateAsync(DOMAIN.Entities.Destinatario destinatario)
        {
            await _context.Destinatarios.AddAsync(destinatario);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Destinatario destinatario)
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
