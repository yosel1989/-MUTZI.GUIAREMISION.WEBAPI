using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Transporte;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class TransporteRepository : ITransporteRepository
    {
        private readonly LogisticaContext _context;
        public TransporteRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Transporte>> GetAllAsync()
        {
            return await _context.Transportes.ToListAsync();
        }
        public async Task<List<Transporte>> GetListBySuggestion(string numeroDoc)
        {
            return await _context.Transportes
                .Where(c => c.IdEstado == 1 &&
                (c.Descripcion.Contains(numeroDoc) || c.Placa.Contains(numeroDoc)))
                .ToListAsync();
        }
        public async Task<(List<Transporte>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query = _context.Transportes.AsQueryable();
            if (request.Columns != null && request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.Descripcion.ToLower().Contains(val) ||
                    x.Marca.ToLower().Contains(val)||x.Modelo.ToLower().Contains(val)||
                    x.Placa.ToLower().Contains(val)||x.NumeroRegistroMtc.ToLower().Contains(val)||
                    x.Tarjeta.ToLower().Contains(val)||x.EmpleadoNombreCreacion.ToLower().Contains(val)||
                    x.EmpleadoNombreEdicion.ToLower().Contains(val) || x.Estado.ToLower().Contains(val)
                    );
                }
            }
            var totalRecords = await query.CountAsync();
            //var query = _context.Transportes.OrderByDescending(t => t.FechaCreacion);

            var data = await query
                .OrderByDescending(t=>t.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.Transporte?> GetByIdAsync(int id)
        {
            return await _context.Transportes.FirstOrDefaultAsync(t => t.TransporteId == id);
        }
        public async Task CreateAsync(DOMAIN.Entities.Transporte transporte)
        {
            await _context.Transportes.AddAsync(transporte);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Transporte transporte)
        {
            _context.Transportes.Update(transporte);
            transporte.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var transporte = await _context.Transportes.FindAsync(id);
            if (transporte != null)
            {
                _context.Transportes.Remove(transporte);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
