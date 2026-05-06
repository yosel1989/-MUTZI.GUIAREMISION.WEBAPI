
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
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

        public async Task<List<Proveedor>> GetAllAsync()
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

        public async Task<(List<Proveedor>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var query = _context.Proveedors
                .Include(p => p.Ubigeo)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var val = search.ToLower().Trim();
                query = query.Where(x => x.TipoDocumento.ToLower().Contains(val) || 
                    x.NumeroDocumento.Contains(val) || 
                    x.RazonSocial.ToLower().Contains(val) || 
                    x.Ubigeo.Departamento.ToLower().Contains(val) || 
                    x.Ubigeo.Provincia.ToLower().Contains(val) || 
                    x.Ubigeo.Distrito.ToLower().Contains(val) || 
                    x.Direccion.ToLower().Contains(val) || 
                    x.Email.ToLower().Contains(val) || 
                    x.Pais.ToLower().Contains(val) || 
                    x.CodigoSunat.ToLower().Contains(val) || 
                    x.UsuarioRegistroNombre.ToLower().Contains(val) || 
                    x.Estado.ToLower().Contains(val) || 
                    x.UsuarioModificoNombre.ToLower().Contains(val)
                );
            }
            
            var totalRecords = await query.CountAsync();
            
            var data = await query
                .OrderByDescending(p => p.FechaRegistro)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (data, totalRecords);
        }

        public async Task<Proveedor?> GetByIdAsync(int id)
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .FirstOrDefaultAsync(p => p.ProveedorId == id);
        }

        public async Task<Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .FirstOrDefaultAsync(p => p.NumeroDocumento == numeroDocumento);
        }

        public async Task CreateAsync(Proveedor proveedor)
        {
            await _context.Proveedors.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task<Proveedor> UpdateAsync(Proveedor proveedor)
        {
            _context.Proveedors.Update(proveedor);
            proveedor.FechaModifico = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await _context.Entry(proveedor).ReloadAsync();

            return proveedor;
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
