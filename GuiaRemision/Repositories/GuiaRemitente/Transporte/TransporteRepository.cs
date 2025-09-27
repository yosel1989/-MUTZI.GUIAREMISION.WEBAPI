
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.GuiaRemitente.Transporte
{
    public class TransporteRepository : ITransporteRepository
    {
        private readonly LogisticaContext _context;
        public TransporteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Models.DBMS.Transporte>> GetAllAsync()
        {
            return await _context.Transportes.ToListAsync();
        }
        public async Task<Models.DBMS.Transporte?> GetByIdAsync(int id)
        {
            return await _context.Transportes.FirstOrDefaultAsync(t => t.TransporteId == id);
        }
        public async Task CreateAsync(Models.DBMS.Transporte transporte)
        {
            await _context.Transportes.AddAsync(transporte);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.DBMS.Transporte transporte)
        {
            _context.Transportes.Update(transporte);
            transporte.FechaEdicion = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var transporte = await _context.Transportes.FindAsync(id);
            if (transporte != null)
            {
                _context.Transportes.Remove(transporte);
                await _context.SaveChangesAsync();
            }
        }
    }
}
