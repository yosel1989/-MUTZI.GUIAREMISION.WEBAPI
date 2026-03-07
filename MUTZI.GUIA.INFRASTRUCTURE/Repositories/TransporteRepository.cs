using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class TransporteRepository : ITransporteRepository
    {
        private readonly LogisticaContext _context;
        public TransporteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Transporte>> GetAllAsync()
        {
            return await _context.Transportes.ToListAsync();
        }
        public async Task<(List<Transporte>, int)> GetListAsync(int pageNumber, int pageSize)
        {
            var query = _context.Transportes.OrderBy(t => t.TransporteId);
            var totalRecords = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.Transporte?> GetByIdAsync(int id)
        {
            return await _context.Transportes.FirstOrDefaultAsync(t => t.TransporteId == id);
        }
        public async Task CreateAsync(DOMAIN.Entities.Transporte transporte)
        {
            await _context.Transportes.AddAsync(transporte);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Transporte transporte)
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
