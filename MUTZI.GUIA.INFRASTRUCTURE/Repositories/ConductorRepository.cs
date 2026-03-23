using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Conductor;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;

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
        public async Task<List<Conductor>> GetListBySuggestion(string numeroDoc)
        {
            return await _context.Conductors
                .Where(c=>c.IdEstado==1 && 
                (c.NumeroDocumento.Contains(numeroDoc)||c.Nombres.Contains(numeroDoc)||c.Apellidos.Contains(numeroDoc)))
                .ToListAsync();
        }
        public async Task<(List<Conductor>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query=_context.Conductors.AsQueryable();
            if (request.Columns!=null&&request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.TipoDocumento.ToLower().Contains(val)||
                    x.NumeroDocumento.ToLower().Contains(val)||
                    x.Nombres.ToLower().Contains(val)|| x.Apellidos.ToLower().Contains(val)||
                    x.Cargo.ToLower().Contains(val)|| x.Licencia.ToLower().Contains(val)||
                    x.EmpleadoNombreCreacion.ToLower().Contains(val)||x.Estado.ToLower().Contains(val)||
                    x.EmpleadoNombreEdicion.ToLower().Contains(val)
                    );
                }
            }
            //var query = _context.Conductors.OrderByDescending(x=>x.FechaCreacion);
            var totalRecords=await query.CountAsync();
            var data = await query
                .OrderByDescending(x=>x.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.Conductor?> GetByIdAsync(int id)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.ConductorId == id);
        }
        public async Task<Conductor?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Conductors.FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
        }
        public async Task CreateAsync(DOMAIN.Entities.Conductor conductor)
        {
            await _context.Conductors.AddAsync(conductor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Conductor conductor)
        {
            _context.Conductors.Update(conductor);
            conductor.FechaEdicion = DateTime.UtcNow;
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