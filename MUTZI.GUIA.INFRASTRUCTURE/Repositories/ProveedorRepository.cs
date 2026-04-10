
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly LogisticaContext _context;
        public ProveedorRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Proveedor>> GetAllAsync()
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .ToListAsync();
        }
        public async Task<List<Proveedor>> GetListBySuggestion(string numeroDoc)
        {
            var query = _context.Proveedors.Where(c => c.IdEstado == 1);
            if (!string.IsNullOrEmpty(numeroDoc))
            {
                query = query.Where(c =>
                    c.NumeroDocumento.Contains(numeroDoc) ||
                    c.RazonSocial.Contains(numeroDoc));
            }
            return await query.ToListAsync();
        }
        public async Task<(List<Proveedor>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query=_context.Proveedors.AsQueryable();
            if (request.Columns != null && request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.TipoDocumento.ToLower().Contains(val) ||
                    x.NumeroDocumento.Contains(val)||x.RazonSocial.ToLower().Contains(val)||
                    x.Ubigeo.Departamento.ToLower().Contains(val)|| x.Ubigeo.Provincia.ToLower().Contains(val)||
                    x.Ubigeo.Distrito.ToLower().Contains(val)|| x.Direccion.ToLower().Contains(val)||
                    x.Email.ToLower().Contains(val)||x.Pais.ToLower().Contains(val)||
                    x.CodigoSunat.ToLower().Contains(val)|| x.EmpleadoNombreCreacion.ToLower().Contains(val) ||
                    x.Estado.ToLower().Contains(val) ||x.EmpleadoNombreEdicion.ToLower().Contains(val)
                    );
                }
            }
            var totalRecords = await query.CountAsync();
            //var query = _context.Proveedors
            //    .Include(p => p.Ubigeo)
            //    .OrderByDescending(p=>p.FechaCreacion);
            
            var data = await query
                .OrderByDescending(p => p.FechaCreacion)
                .Include(p => p.Ubigeo)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.Proveedor?> GetByIdAsync(int id)
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .FirstOrDefaultAsync(p => p.ProveedorId == id);
        }
        public async Task<DOMAIN.Entities.Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .FirstOrDefaultAsync(p => p.NumeroDocumento == numeroDocumento);
        }
        public async Task CreateAsync(DOMAIN.Entities.Proveedor proveedor)
        {
            await _context.Proveedors.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Proveedor proveedor)
        {
            _context.Proveedors.Update(proveedor);
            proveedor.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedors.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }

       
    }
}
