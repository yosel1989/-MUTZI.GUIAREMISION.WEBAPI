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

        public async Task<List<TransportUnit>> GetAllAsync()
        {
            try
            {
                var data = await _context.UnidadTransporte.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransportUnit>> GetListBySuggestion(string numeroDoc)
        {
            try
            {
                var query = _context.UnidadTransporte
                    .Where(c => c.Active);
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

        public async Task<(List<TransportUnit>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.UnidadTransporte
                    .Include(m => m.EntidadReguladoraVehicular)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower().Trim();
                    query = query.Where(x => x.Descripcion.ToLower().Contains(val) ||
                    x.Marca.ToLower().Contains(val) ||
                    x.Modelo.ToLower().Contains(val) ||
                    x.Placa.ToLower().Contains(val) ||
                    x.NroAutorizacion.ToLower().Contains(val) ||
                    x.EntidadReguladoraVehicular.Descripcion.ToLower().Contains(val) ||
                    x.EntidadReguladoraVehicular.DescripcionCorta.ToLower().Contains(val) ||
                    x.Tarjeta.ToLower().Contains(val) ||
                    x.CreatedAtUserName.ToLower().Contains(val) ||
                    x.UpdatedAtUserName.ToLower().Contains(val)
                );
                }

                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(t => t.CreatedAt)
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

        public async Task<TransportUnit?> GetByIdAsync(int id)
        {
            try
            {
                var unidadTransporte = await _context.UnidadTransporte
                    .Include(m => m.EntidadReguladoraVehicular)
                    .FirstOrDefaultAsync(t => t.UnidadTransporteId == id);
                return unidadTransporte;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateAsync(TransportUnit transporte)
        {
            try
            {
                await _context.UnidadTransporte.AddAsync(transporte);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<TransportUnit> UpdateAsync(TransportUnit transporte)
        {
            try
            {
                _context.UnidadTransporte.Update(transporte);
                await _context.SaveChangesAsync();

                var unidadTransporteActualizado = await GetByIdAsync(transporte.UnidadTransporteId);

                return unidadTransporteActualizado!;
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
                var transporte = await _context.UnidadTransporte.FindAsync(id);
                if (transporte != null)
                {
                    _context.UnidadTransporte.Remove(transporte);
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
