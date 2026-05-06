using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class UnidadTransporteRepository : IUnidadTransporteRepository
    {
        private readonly LogisticaContext _context;
        public UnidadTransporteRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<UnidadTransporte>> GetAllAsync()
        {
            try
            {
                var data = await _context.UnidadTransportes.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UnidadTransporte>> GetListBySuggestion(string numeroDoc)
        {
            try
            {
                var query = _context.UnidadTransportes.Where(c => c.IdEstado == 1);
                if (!string.IsNullOrEmpty(numeroDoc))
                {
                    query = query.Where(c =>
                        c.Descripcion.Contains(numeroDoc) ||
                        c.Placa.Contains(numeroDoc) ||
                        c.Tarjeta.Contains(numeroDoc));
                }
                var data = await query.ToListAsync();
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        public async Task<(List<UnidadTransporte>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.UnidadTransportes.AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower().Trim();
                    query = query.Where(x => x.Descripcion.ToLower().Contains(val) ||
                    x.Marca.ToLower().Contains(val) ||
                    x.Modelo.ToLower().Contains(val) ||
                    x.Placa.ToLower().Contains(val) ||
                    x.NroAutorizacion.ToLower().Contains(val) ||
                    x.EmisorVehicular.ToLower().Contains(val) ||
                    x.Tarjeta.ToLower().Contains(val) ||
                    x.UsuarioRegistroNombre.ToLower().Contains(val) ||
                    x.UsuarioModificoNombre.ToLower().Contains(val) ||
                    x.Estado.ToLower().Contains(val)
                );
                }

                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(t => t.FechaRegistro)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                return (data, totalRecords);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UnidadTransporte?> GetByIdAsync(int id)
        {
            try
            {
                var unidadTransporte = await _context.UnidadTransportes.FirstOrDefaultAsync(t => t.UnidadTransporteId == id);
                return unidadTransporte;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateAsync(UnidadTransporte transporte)
        {
            try
            {
                await _context.UnidadTransportes.AddAsync(transporte);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<UnidadTransporte> UpdateAsync(UnidadTransporte transporte)
        {
            try
            {
                _context.UnidadTransportes.Update(transporte);
                await _context.SaveChangesAsync();
                await _context.Entry(transporte).ReloadAsync();

                return transporte;
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
                var transporte = await _context.UnidadTransportes.FindAsync(id);
                if (transporte != null)
                {
                    _context.UnidadTransportes.Remove(transporte);
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
