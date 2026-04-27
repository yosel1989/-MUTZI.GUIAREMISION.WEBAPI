using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class EstablecimientoRepository: IEstablecimientoRepository
    {
        private readonly LogisticaContext _context;
        public EstablecimientoRepository(LogisticaContext context)
        {
            _context = context;
        }
        public async Task<List<Establecimiento>> GetAllByRucToSelectAsync(string ruc)
        {
            return await _context.Establecimiento
                .Where(e => e.Ruc == ruc && e.IdEstado == 1)
                .Include(r=>r.Ubigeo)
                .OrderBy(e => e.Descripcion)
                .ToListAsync();
        }
        public async Task<List<Establecimiento>> GetAllByRucToSelectModal(string ruc)
        {
            return await _context.Establecimiento
            .Select(r => new Establecimiento
            {
                EstablecimientoId = r.EstablecimientoId,
                Descripcion = r.Descripcion,
                CodigoSunat = r.CodigoSunat
            })
            .ToListAsync();
        }

        public async Task<List<Establecimiento>> GetListBySuggestion(string ruc, string? search)
        {
            return await _context.Establecimiento
                .Include(r => r.Ubigeo)
                .Where(c =>
                    c.IdEstado == 1 &&
                    (string.IsNullOrEmpty(search) ||
                     c.CodigoSunat.Contains(search) ||
                     c.Descripcion.Contains(search)) &&
                    c.Ruc == ruc
                )
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Establecimiento?> GetByIdAsync(int id)
        {
            return await _context.Establecimiento
                .Include(r => r.Ubigeo)
                .Include(r => r.RucNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.EstablecimientoId == id);
        }


        /*
        public async Task<List<Remitente>> GetListBySuggestion(string numeroDoc)
        {
            return await _context.Remitentes
                .Include(r => r.Ubigeo)
                .Include(e => e.RucNavigation)
                .Where(c => c.IdEstado == 1 &&
                (c.Ruc.Contains(numeroDoc) || c.Descripcion.Contains(numeroDoc)))
                .ToListAsync();
        }
        public async Task<(List<Remitente>, int)> GetListAsync(int pageNumber, int pageSize, DataTableDTO request)
        {
            var query=_context.Remitentes.AsQueryable();
            if (request.Columns != null && request.Columns.Any())
            {
                var val = request.Columns[0].Search.Value?.ToLower();
                var key = request.Columns[0].Data?.ToLower();
                if (!string.IsNullOrEmpty(val))
                {
                    query = query.Where(x => x.RucNavigation.Empresa.ToLower().Contains(val) ||
                    x.Ruc.Contains(val)||x.Descripcion.ToLower().Contains(val)||
                    x.Ubigeo.Departamento.ToLower().Contains(val) || x.Ubigeo.Provincia.ToLower().Contains(val) ||
                    x.Ubigeo.Distrito.ToLower().Contains(val) || x.Direccion.ToLower().Contains(val) ||
                    x.Email.ToLower().Contains(val) || x.Pais.ToLower().Contains(val) ||x.Serie.Contains(val)||
                    x.CodigoSunat.ToLower().Contains(val) || x.EmpleadoNombreCreacion.ToLower().Contains(val) ||
                    x.Estado.ToLower().Contains(val) || x.EmpleadoNombreEdicion.ToLower().Contains(val)
                    );
                }
            }
            var totalRecords = await query.CountAsync();
            //var query = _context.Remitentes
            //    .Include(r => r.Ubigeo)
            //    .Include(e => e.RucNavigation)
            //    .OrderByDescending(r=>r.FechaCreacion);

            var data = await query
                .OrderByDescending(r => r.FechaCreacion)
                .Include(r => r.Ubigeo)
                .Include(e => e.RucNavigation)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }
        public async Task<DOMAIN.Entities.GuiaRemision?> GetLastBySerieAsync(string serie)
        {
            if (string.IsNullOrWhiteSpace(serie))
                return null;

            return await _context.GuiaRemisions
                .Where(g => g.Serie == serie)
                .OrderByDescending(g => g.Correlativo)
                .FirstOrDefaultAsync();
        }
        public async Task<DOMAIN.Entities.Remitente?> GetByIdAsync(int id)
        {
            return await _context.Remitentes
                .Include(r => r.Ubigeo)
                .Include(e => e.RucNavigation)
                .FirstOrDefaultAsync(r => r.RemitenteId == id);
        }
        public async Task<bool> GetBySerieAsync(string ruc, string serie)
        {
            var existe = await _context.Remitentes
                .AnyAsync(r => r.Ruc == ruc && r.Serie == serie);
            return existe;
        }
        public async Task CreateAsync(DOMAIN.Entities.Remitente remitente)
        {
            await _context.Remitentes.AddAsync(remitente);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DOMAIN.Entities.Remitente remitente)
        {
            _context.Remitentes.Update(remitente);
            remitente.FechaEdicion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var remitente = await _context.Remitentes.FindAsync(id);
            if (remitente != null)
            {
                _context.Remitentes.Remove(remitente);
                await _context.SaveChangesAsync();
            }
        }*/
    }
}
