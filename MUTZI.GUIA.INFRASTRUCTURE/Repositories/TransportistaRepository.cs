using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
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

        public async Task<List<Transportista>> GetAllAsync()
        {
            try
            {
                var data = await _context.Transportista.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<List<Transportista>> GetListBySuggestion(string numeroDoc)
        {
            try
            {
                var query = _context.Transportista
                    .Where(c => c.Active)
                    .Select(x => new Transportista
                    {
                        TransportistaId = x.TransportistaId,
                        RazonSocial = x.RazonSocial,
                        NumeroDocumento = x.NumeroDocumento,
                        TipoDocumentoId = x.TipoDocumentoId,
                        TipoDocumento = new TipoDocumento
                        {
                            Descripcion = x.TipoDocumento.Descripcion,
                            DescripcionCorta = x.TipoDocumento.DescripcionCorta
                        }
                    });
                if (!string.IsNullOrEmpty(numeroDoc))
                {
                    query = query.Where(c =>
                        c.RazonSocial.Contains(numeroDoc) ||
                        c.NumeroDocumento.Contains(numeroDoc)
                    );
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<Transportista>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            try
            {
                var query = _context.Transportista
                .Include(x => x.TipoDocumento)
                .Include(x => x.Ubigeo)
                .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower().Trim();
                    query = query.Where(x =>
                        x.NumeroDocumento.Contains(val) ||
                        x.RazonSocial.ToLower().Contains(val) ||
                        x.RegistroMTC.ToLower().Contains(val)
                    );
                }

                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(t => t.TransportistaId)
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

        public async Task<Transportista?> GetByIdAsync(int id)
        {
            try
            {
                var data = await _context.Transportista
                .Include(x => x.TipoDocumento)
                .Include(x => x.Ubigeo)
                .FirstOrDefaultAsync(t => t.TransportistaId == id);

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateAsync(Transportista transporte)
        {
            try
            {
                await _context.Transportista.AddAsync(transporte);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Transportista> UpdateAsync(Transportista transporte)
        {
            try
            {
                _context.Transportista.Update(transporte);
                await _context.SaveChangesAsync();
                
                var transportistaUpdated = await GetByIdAsync(transporte.TransportistaId);
                return transportistaUpdated!;
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
                var transporte = await _context.Transportista.FindAsync(id);
                if (transporte != null)
                {
                    _context.Transportista.Remove(transporte);
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
