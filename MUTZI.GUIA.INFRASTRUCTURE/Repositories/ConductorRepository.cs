using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Conductor;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly LogisticaContext _context;
        public ConductorRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Conductor>> GetAllAsync()
        {
            return await _context.Conductors.ToListAsync();
        }
        public async Task<DOMAIN.Entities.Conductor?> GetByIdAsync(int id)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.ConductorId == id);
        }
        public async Task CreateAsync(DOMAIN.Entities.Conductor conductor)
        {
            await _context.Conductors.AddAsync(conductor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Conductor conductor)
        {
            _context.Conductors.Update(conductor);
            conductor.FechaEdicion = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var conductor = await _context.Conductors.FindAsync(id);
            if (conductor != null)
            {
                _context.Conductors.Remove(conductor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
