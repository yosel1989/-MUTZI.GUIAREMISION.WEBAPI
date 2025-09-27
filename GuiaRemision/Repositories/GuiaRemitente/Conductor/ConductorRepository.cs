
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.GuiaRemitente.Conductor
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly LogisticaContext _context;
        public ConductorRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Models.DBMS.Conductor>> GetAllAsync()
        {
            return await _context.Conductors.ToListAsync();
        }
        public async Task<Models.DBMS.Conductor?> GetByIdAsync(int id)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.ConductorId == id);
        }
        public async Task CreateAsync(Models.DBMS.Conductor conductor)
        {
            await _context.Conductors.AddAsync(conductor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.DBMS.Conductor conductor)
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
