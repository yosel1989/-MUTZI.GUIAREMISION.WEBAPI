using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
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
                var conductor = await _context.Conductors.ToListAsync();
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
                var query = _context.Conductors.Where(c => c.IdEstado == 1);
                if (!string.IsNullOrEmpty(numeroDoc))
                {
                    query = query.Where(c =>
                        c.NumeroDocumento.Contains(numeroDoc) ||
                        c.Nombres.Contains(numeroDoc) ||
                        c.Apellidos.Contains(numeroDoc));
                }
                return await query.ToListAsync();
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
                var query = _context.Conductors.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower().Trim();
                    query = query.Where(x => x.TipoDocumento.ToLower().Contains(val) ||
                        x.NumeroDocumento.ToLower().Contains(val) ||
                        x.Nombres.ToLower().Contains(val) ||
                        x.Apellidos.ToLower().Contains(val) ||
                        x.Cargo.ToLower().Contains(val) ||
                        x.Licencia.ToLower().Contains(val) ||
                        x.UsuarioRegistroNombre.ToLower().Contains(val) ||
                        x.Estado.ToLower().Contains(val) ||
                        x.UsuarioModificoNombre.ToLower().Contains(val)
                    );
                }

                //var query = _context.Conductors.OrderByDescending(x=>x.FechaRegistro);
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
                var conductor = await _context.Conductors.FirstOrDefaultAsync(c => c.ConductorId == id);
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
                var conductor = await _context.Conductors.FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
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
                await _context.Conductors.AddAsync(conductor);
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
                _context.Conductors.Update(conductor);
                conductor.FechaModifico = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await _context.Entry(conductor).ReloadAsync();
                return conductor;
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
                var conductor = await _context.Conductors.FindAsync(id);
                if (conductor != null)
                {
                    _context.Conductors.Remove(conductor);
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