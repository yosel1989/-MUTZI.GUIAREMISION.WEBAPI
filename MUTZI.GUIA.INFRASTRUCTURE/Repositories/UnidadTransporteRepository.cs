using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte;
using MUTZI.GUIA.DOMAIN.Entities;
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
        public async Task<List<DOMAIN.Entities.UnidadTransporte>> GetAllAsync()
        {
            return await _context.UnidadTransportes.ToListAsync();
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
        public async Task<(List<UnidadTransporte>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query = _context.UnidadTransportes.AsQueryable();
            if (request.Columns != null && request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.Descripcion.ToLower().Contains(val) ||
                    x.Marca.ToLower().Contains(val)||x.Modelo.ToLower().Contains(val)||
                    x.Placa.ToLower().Contains(val)||x.NroAutorizacion.ToLower().Contains(val)||
                    x.EmisorVehicular.ToLower().Contains(val) ||
                    x.Tarjeta.ToLower().Contains(val)||x.EmpleadoNombreCreacion.ToLower().Contains(val)||
                    x.EmpleadoNombreEdicion.ToLower().Contains(val) || x.Estado.ToLower().Contains(val)
                    );
                }
            }
            var totalRecords = await query.CountAsync();
            //var query = _context.UnidadTransportes.OrderByDescending(t => t.FechaCreacion);

            var data = await query
                .OrderByDescending(t=>t.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.UnidadTransporte?> GetByIdAsync(int id)
        {
            return await _context.UnidadTransportes.FirstOrDefaultAsync(t => t.TransporteId == id);
        }
        public async Task CreateAsync(DOMAIN.Entities.UnidadTransporte transporte)
        {
            transporte.FechaCreacion = DateTime.UtcNow;
            await _context.UnidadTransportes.AddAsync(transporte);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.UnidadTransporte transporte)
        {
            _context.UnidadTransportes.Update(transporte);
            transporte.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var transporte = await _context.UnidadTransportes.FindAsync(id);
            if (transporte != null)
            {
                _context.UnidadTransportes.Remove(transporte);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
