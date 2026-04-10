
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Repositories.GuiaRemitente.Destinatario;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class DestinatarioRepository : IDestinatarioRepository
    {
        private readonly LogisticaContext _context;
        public DestinatarioRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<DOMAIN.Entities.Destinatario>> GetAllAsync()
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .ToListAsync();
        }
        public async Task<List<Destinatario>> GetListBySuggestion(string numeroDoc)
        {
            var query = _context.Destinatarios
                .Where(c => c.IdEstado == 1);

            if (!string.IsNullOrEmpty(numeroDoc))
            {
                query = query.Where(c =>
                    c.NumeroDocumento.Contains(numeroDoc) ||
                    c.RazonSocial.Contains(numeroDoc));
            }

            return await query.ToListAsync();
        }
        public async Task<(List<Destinatario>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query = _context.Destinatarios.AsQueryable();
            if (request.Columns != null && request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.TipoDocumento.ToLower().Contains(val) ||
                    x.NumeroDocumento.Contains(val) || x.RazonSocial.Contains(val)||
                    x.Ubigeo.Departamento.ToLower().Contains(val)|| x.Ubigeo.Provincia.ToLower().Contains(val)||
                    x.Ubigeo.Distrito.ToLower().Contains(val)||x.Direccion.ToLower().Contains(val)||
                    x.Pais.ToLower().Contains(val) ||x.CodigoSunat.ToLower().Contains(val)||
                    x.EmpleadoNombreCreacion.ToLower().Contains(val) || x.Estado.ToLower().Contains(val)||
                    x.EmpleadoNombreEdicion.ToLower().Contains(val)
                    );
                }
            }
            var totalRecords = await query.CountAsync();

            //var query = _context.Destinatarios
            //    .Include(d => d.Ubigeo)
            //    .OrderByDescending(d=>d.FechaCreacion);
            
            var data = await query
                .OrderByDescending(x=>x.FechaCreacion)
                .Include(d=>d.Ubigeo)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.Destinatario?> GetByIdAsync(int id)
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .FirstOrDefaultAsync(d => d.DestinatarioId == id);
        }
        public async Task<Destinatario?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Destinatarios
                .Include(d => d.Ubigeo)
                .FirstOrDefaultAsync(d => d.NumeroDocumento == numeroDocumento);
        }
        public async Task CreateAsync(DOMAIN.Entities.Destinatario destinatario)
        {
            await _context.Destinatarios.AddAsync(destinatario);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Destinatario destinatario)
        {
            _context.Destinatarios.Update(destinatario);
            destinatario.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var destinatario = await _context.Destinatarios.FindAsync(id);
            if (destinatario != null)
            {
                _context.Destinatarios.Remove(destinatario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
