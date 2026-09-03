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

        public async Task<List<EntityBranch>> GetAllByRucToSelectAsync(string ruc)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Where(e => e.Ruc == ruc && e.EstadoId == 1)
                    .Include(r=>r.Ubigeo)
                    .OrderBy(e => e.Description)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        public async Task<List<EntityBranch>> GetAllByRucToSelect2Async(string ruc)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Include(r => r.Ubigeo)
                    .Where(e => e.Ruc == ruc && e.EstadoId == 1)
                    .Select(m => new EntityBranch
                    {
                        EntityBranchId = m.EntityBranchId,
                        Description = m.Description,
                        Area = m.Area
                    })
                    .OrderBy(e => e.Description)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<EntityBranch>> GetAllByRucToSelectModal(string ruc)
        {
            try
            {
                var data = await _context.Establecimiento
                .Select(r => new EntityBranch
                {
                    EntityBranchId = r.EntityBranchId,
                    Description = r.Description,
                    CodeSunat = r.CodeSunat,
                    Area = r.Area
                })
                .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EntityBranch>> GetListBySuggestion(string ruc, string? search)
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
                         c.CodeSunat.Contains(search) ||
                         c.Description.Contains(search) ||
                         c.Area.Contains(search)
                        )
                    );
                }

                var data = await query
                .Where(q => q.Ruc == ruc)
                .Where(q => q.EstadoId == 1)
                .Select(m => new EntityBranch
                {
                    EntityBranchId = m.EntityBranchId,
                    Description = m.Description,
                    CodeSunat = m.CodeSunat,
                    Area = m.Area,
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

        public async Task<EntityBranch?> GetByIdAsync(int id, bool strict, string? tipo)
        {
            try
            {
                var data = await _context.Establecimiento
                    .Include(r => r.Ubigeo)
                    .Include(r => r.RucNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.EntityBranchId == id);

                if(strict && data is null)
                {
                    throw new Exception($"No existe el establecimiento ${ (tipo is null ? "" : "del " + tipo) } en los registros.");
                }

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<(List<EntityBranch>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.Establecimiento
                .Include(r => r.Ubigeo)
                .Include(e => e.RucNavigation)
                .Include(e => e.Entity)
                .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(x =>
                        x.Entity.DocumentNumber.Contains(val) ||
                        x.Entity.Name.ToLower().Contains(val) ||
                        x.Entity.FirstName.ToLower().Contains(val) ||
                        x.Entity.LastName.ToLower().Contains(val) ||
                        x.Description.ToLower().Contains(val) ||
                        x.Ubigeo.Departamento.ToLower().Contains(val) ||
                        x.Ubigeo.Provincia.ToLower().Contains(val) ||
                        x.Ubigeo.Distrito.ToLower().Contains(val) ||
                        x.Address.ToLower().Contains(val) ||
                        x.Email.ToLower().Contains(val) ||
                        x.Pais.ToLower().Contains(val) ||
                        x.Serie.Contains(val) ||
                        x.CodeSunat.ToLower().Contains(val) ||
                        x.UsuarioRegistroNombre.ToLower().Contains(val) ||
                        x.Estado.ToLower().Contains(val) ||
                        x.Area.ToLower().Contains(val) ||
                        x.UsuarioModificoNombre.ToLower().Contains(val)
                    );
                }
                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(r => r.EntityBranchId)
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

        public async Task<EntityBranch> UpdateAsync(EntityBranch actualizarEstablecimiento)
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

        public async Task CreateAsync(EntityBranch nuevoEstablecimiento)
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
