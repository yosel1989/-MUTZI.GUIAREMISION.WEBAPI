using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class PerfilRepository: IPerfilRepository
    {
        private readonly LogisticaContext _context;
        public PerfilRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<Perfil?> GetByIdAsync(int id)
        {
            return await _context.Perfil
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.PerfilId == id);
        }

        public async Task<(List<Perfil>, int)> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var query = _context.Perfil
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                var val = search.ToLower();
                query = query.Where(x =>
                    x.Nombre.ToLower().Contains(val) ||
                    x.Descripcion.Contains(val) ||
                    x.Estado.ToLower().Contains(val) ||
                    x.UsuarioRegistro.ToLower().Contains(val) ||
                    x.UsuarioModifico.ToLower().Contains(val)
                );
            }
            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(r => r.PerfilId)
                .Skip((pageNumber - 1) * pageSize)
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync();

            return (data, totalRecords);
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.Perfil.FindAsync(id);
            if (model != null)
            {
                _context.Perfil.Remove(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Perfil> UpdateAsync(Perfil actualizarPerfil)
        {
            var entity = await _context.Perfil
                .FirstOrDefaultAsync(e => e.PerfilId == actualizarPerfil.PerfilId) ??
                throw new KeyNotFoundException($"Perfil con ID {actualizarPerfil.PerfilId} no encontrado.");

            if (entity != null)
            {
                entity.Nombre = actualizarPerfil.Nombre;
                entity.Descripcion = actualizarPerfil.Descripcion;
                entity.Codigo = actualizarPerfil.Codigo;
                entity.UsuarioModifico = actualizarPerfil.UsuarioModifico;
                entity.FechaModifico = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task CreateAsync(Perfil nuevoPerfil)
        {
            await _context.Perfil.AddAsync(nuevoPerfil);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Perfil>> GetListToSelectAsync()
        {
            return await _context.Perfil
                .Where(m => m.EstadoId == 1)
                .Select(m => new Perfil
                {
                    PerfilId = m.PerfilId,
                    Nombre = m.Nombre
                })
                .ToListAsync();
        }

    }
}
