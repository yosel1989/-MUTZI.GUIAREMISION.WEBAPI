using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly LogisticaContext _context;
        public ConductorRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Conductor>> GetAllAsync()
        {
            try
            {
                var conductor = await _context.Conductor.ToListAsync();
                return conductor;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public async Task<List<Conductor>> GetListBySuggestion(string numeroDoc)
        {
            try
            {
                var query = _context.Conductor.Where(c => c.Active);
                if (!string.IsNullOrEmpty(numeroDoc))
                {
                    query = query.Where(c =>
                        c.NumeroDocumento.Contains(numeroDoc) ||
                        c.Nombres.Contains(numeroDoc) ||
                        c.Apellidos.Contains(numeroDoc)
                    );
                }
                var data = await query.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<(List<Conductor>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.Conductor.Include(m => m.TipoDocumento).AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower().Trim();
                    query = query.Where(x => 
                        x.TipoDocumento.Descripcion.ToLower().Contains(val) ||
                        x.TipoDocumento.DescripcionCorta.ToLower().Contains(val) ||
                        x.NumeroDocumento.ToLower().Contains(val) ||
                        x.Nombres.ToLower().Contains(val) ||
                        x.Apellidos.ToLower().Contains(val) ||
                        x.Cargo.ToLower().Contains(val) ||
                        x.Licencia.ToLower().Contains(val) ||
                        x.UsuarioRegistroNombre.ToLower().Contains(val) ||
                        x.UsuarioModificoNombre.ToLower().Contains(val)
                    );
                }

                //var query = _context.Conductor.OrderByDescending(x=>x.CreatedAt);
                var totalRecords = await query.CountAsync();
                var data = await query
                    .OrderByDescending(x => x.FechaRegistro)
                    .Skip((pageNumber - 1) * pageSize)
                    .AsNoTracking()
                    .Take(pageSize)
                    .ToListAsync();

                return (data, totalRecords);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Conductor?> GetByIdAsync(int id)
        {
            try
            {
                var conductor = await _context.Conductor.Include(m => m.TipoDocumento).FirstOrDefaultAsync(c => c.ConductorId == id);
                return conductor;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Conductor?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            try
            {
                var conductor = await _context.Conductor.FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
                return conductor;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task CreateAsync(Conductor conductor)
        {
            try
            {
                await _context.Conductor.AddAsync(conductor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Conductor> UpdateAsync(Conductor conductor)
        {
            try
            {
                _context.Conductor.Update(conductor);
                conductor.FechaModifico = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var conductorModificado = await GetByIdAsync(conductor.ConductorId);
                await _context.Entry(conductorModificado).ReloadAsync();
                return conductorModificado!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var conductor = await _context.Conductor.FindAsync(id);
                if (conductor != null)
                {
                    _context.Conductor.Remove(conductor);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}