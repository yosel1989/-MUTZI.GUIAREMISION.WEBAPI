using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class OperacionResponsableRepository: IOperacionResponsableRepository
    {
        private readonly LogisticaContext _context;
        public OperacionResponsableRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OperacionResponsable>> GetListToSelectAsync()
        {
            return await _context.OperacionResponsable
                .Select(x => new OperacionResponsable
                {
                    OperacionResponsableId = x.OperacionResponsableId,
                    Descripcion = x.Descripcion
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OperacionResponsable> CreateAsync(OperacionResponsable newModel)
        {
            await _context.OperacionResponsable.AddAsync(newModel);
            await _context.SaveChangesAsync();

            await _context.OperacionResponsable.Entry(newModel).ReloadAsync();

            return newModel;
        }

        public async Task<OperacionResponsable?> GetByIdAsync(int id)
        {
            return await _context.OperacionResponsable
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.OperacionResponsableId == id);
        }

        public async Task<(List<OperacionResponsable>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var query = _context.OperacionResponsable
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                var val = search.ToLower();
                query = query.Where(x =>
                    x.Descripcion.Contains(val) ||
                    x.UsuarioRegistro.ToLower().Contains(val) ||
                    x.UsuarioModifico.ToLower().Contains(val)
                );
            }
            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(r => r.OperacionResponsableId)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }

        public async Task<OperacionResponsable> UpdateAsync(OperacionResponsable actualizarModel)
        {
            var entity = await _context.OperacionResponsable
                .FirstOrDefaultAsync(e => e.OperacionResponsableId == actualizarModel.OperacionResponsableId) ??
                throw new KeyNotFoundException($"Operación con ID {actualizarModel.OperacionResponsableId} no encontrado.");

            if (entity != null)
            {
                entity.Descripcion = actualizarModel.Descripcion;
                entity.PersonalModificoId = actualizarModel.PersonalModificoId;
                entity.UsuarioModifico = actualizarModel.UsuarioModifico;
                entity.FechaModifico = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return entity;
        }
    }
}
