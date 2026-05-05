using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.Transportista;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class TransportistaRepository: ITransportistaRepository
    {
        private readonly LogisticaContext _context;
        public TransportistaRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<RemiteTransportistum>> GetAllAsync()
        {
            return await _context.RemiteTransportista.ToListAsync();
        }
        public async Task<List<RemiteTransportistum>> GetListBySuggestion(string numeroDoc)
        {
            var query = _context.RemiteTransportista.Where(c => c.IdEstado == 1);
            if (!string.IsNullOrEmpty(numeroDoc))
            {
                query = query.Where(c =>
                    c.RazonSocial.Contains(numeroDoc) ||
                    c.NumeroDocumento.Contains(numeroDoc)
                );
            }
            return await query.ToListAsync();
        }
        public async Task<(List<RemiteTransportistum>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query = _context.RemiteTransportista
                .Include(x => x.Ubigeo)
                .AsQueryable();
            if (request.Columns != null && request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.NumeroDocumento.Contains(val) ||
                    x.RazonSocial.ToLower().Contains(val)||
                    x.RegistroMTC.ToLower().Contains(val)
                    );
                }
            }
            var totalRecords = await query.CountAsync();
            //var query = _context.UnidadTransportes.OrderByDescending(t => t.FechaRegistro);

            var data = await query
                .OrderByDescending(t=>t.FechaCreacion)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<RemiteTransportistum> GetByIdAsync(int id)
        {
            return await _context.RemiteTransportista.FirstOrDefaultAsync(t => t.RemiteTransportistaId == id);
        }
        public async Task CreateAsync(RemiteTransportistum transporte)
        {
            await _context.RemiteTransportista.AddAsync(transporte);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(RemiteTransportistum transporte)
        {
            _context.RemiteTransportista.Update(transporte);
            transporte.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var transporte = await _context.RemiteTransportista.FindAsync(id);
            if (transporte != null)
            {
                _context.RemiteTransportista.Remove(transporte);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
