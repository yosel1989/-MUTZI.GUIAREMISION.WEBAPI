
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Proveedor;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;

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
        public async Task<(List<Proveedor>, int)> GetListAsync(int pageNumber, int pageSize)
        {
            var query = _context.Proveedors
                .Include(p => p.Ubigeo)
                .OrderBy(p=>p.ProveedorId);
            var totalRecords = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
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
            proveedor.FechaEdicion = DateTime.Now;
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
