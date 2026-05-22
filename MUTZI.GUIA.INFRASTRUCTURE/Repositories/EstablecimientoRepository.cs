using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class EstablecimientoRepository: IEstablecimientoRepository
    {
        private readonly LogisticaContext _context;
        public EstablecimientoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<Establecimiento>> GetAllByRucToSelectAsync(string ruc)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Where(e => e.Ruc == ruc && e.EstadoId == 1)
                    .Include(r=>r.Ubigeo)
                    .OrderBy(e => e.Descripcion)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        public async Task<List<Establecimiento>> GetAllByRucToSelect2Async(string ruc)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Include(r => r.Ubigeo)
                    .Where(e => e.Ruc == ruc && e.EstadoId == 1)
                    .Select(m => new Establecimiento
                    {
                        EstablecimientoId = m.EstablecimientoId,
                        Descripcion = m.Descripcion
                    })
                    .OrderBy(e => e.Descripcion)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<Establecimiento>> GetAllByRucToSelectModal(string ruc)
        {
            try
            {
                var data = await _context.Establecimiento
                .Select(r => new Establecimiento
                {
                    EstablecimientoId = r.EstablecimientoId,
                    Descripcion = r.Descripcion,
                    CodigoSunat = r.CodigoSunat
                })
                .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Establecimiento>> GetListBySuggestion(string ruc, string? search)
        {
            try
            {
                var query = _context.Establecimiento
                .Include(r => r.RucNavigation)
                .AsQueryable();

                if (!String.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        (string.IsNullOrEmpty(search) ||
                         c.CodigoSunat.Contains(search) ||
                         c.Descripcion.Contains(search))
                    );
                }

                var data = await query
                .Where(q => q.Ruc == ruc)
                .Where(q => q.EstadoId == 1)
                .Select(m => new Establecimiento
                {
                    EstablecimientoId = m.EstablecimientoId,
                    Descripcion = m.Descripcion,
                    CodigoSunat = m.CodigoSunat,
                    RucNavigation = new VentaConfig
                    {
                        Empresa = m.RucNavigation.Empresa
                    }
                })
                .AsNoTracking()
                .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Establecimiento?> GetByIdAsync(int id)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Include(r => r.Ubigeo)
                    .Include(r => r.RucNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.EstablecimientoId == id);

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<(List<Establecimiento>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.Establecimiento
                .Include(r => r.Ubigeo)
                .Include(e => e.RucNavigation)
                .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(x =>
                        x.RucNavigation.Empresa.ToLower().Contains(val) ||
                        x.Ruc.Contains(val) ||
                        x.Descripcion.ToLower().Contains(val) ||
                        x.Ubigeo.Departamento.ToLower().Contains(val) ||
                        x.Ubigeo.Provincia.ToLower().Contains(val) ||
                        x.Ubigeo.Distrito.ToLower().Contains(val) ||
                        x.Direccion.ToLower().Contains(val) ||
                        x.Email.ToLower().Contains(val) ||
                        x.Pais.ToLower().Contains(val) ||
                        x.Serie.Contains(val) ||
                        x.CodigoSunat.ToLower().Contains(val) ||
                        x.UsuarioRegistroNombre.ToLower().Contains(val) ||
                        x.Estado.ToLower().Contains(val) ||
                        x.UsuarioModificoNombre.ToLower().Contains(val)
                    );
                }
                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(r => r.EstablecimientoId)
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

        public async Task DeleteAsync(int id)
        {
            try
            {
                var model = await _context.Establecimiento.FindAsync(id);
                if (model != null)
                {
                    _context.Establecimiento.Remove(model);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<Establecimiento> UpdateAsync(Establecimiento actualizarEstablecimiento)
        {
            try
            {
                _context.Establecimiento.Update(actualizarEstablecimiento);
                actualizarEstablecimiento.FechaModifico = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await _context.Entry(actualizarEstablecimiento).ReloadAsync();
                return actualizarEstablecimiento;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateAsync(Establecimiento nuevoEstablecimiento)
        {
            try
            {
                await _context.Establecimiento.AddAsync(nuevoEstablecimiento);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> GetBySerieAsync(string ruc, string serie)
        {
            try
            {
                var existe = await _context.Establecimiento
                .AnyAsync(r => r.Ruc == ruc && r.Serie == serie);
                return existe;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GuiaRemision?> GetLastBySerieAsync(string serie, string ruc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serie))
                    return null;

                if (string.IsNullOrWhiteSpace(ruc))
                    return null;

                var data = await _context.GuiaRemision
                    .FirstOrDefaultAsync() ?? null;

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
