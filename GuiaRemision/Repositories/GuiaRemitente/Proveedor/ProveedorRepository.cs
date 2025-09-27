
using GuiaRemision.Models.DBMS;
using Microsoft.EntityFrameworkCore;

namespace GuiaRemision.Repositories.GuiaRemitente.Proveedor
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly LogisticaContext _context;
        public ProveedorRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Models.DBMS.Proveedor>> GetAllAsync()
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .ToListAsync();
        }
        public async Task<Models.DBMS.Proveedor?> GetByIdAsync(int id)
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .FirstOrDefaultAsync(p => p.ProveedorId == id);
        }
        public async Task<Models.DBMS.Proveedor?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Proveedors
                .Include(p => p.Ubigeo)
                .FirstOrDefaultAsync(p => p.NumeroDocumento == numeroDocumento);
        }
        public async Task CreateAsync(Models.DBMS.Proveedor proveedor)
        {
            await _context.Proveedors.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Models.DBMS.Proveedor proveedor)
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
