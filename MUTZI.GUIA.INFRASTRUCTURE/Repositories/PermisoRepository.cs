using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class PermisoRepository: IPermisoRepository
    {
        private readonly LogisticaContext _context;
        public PermisoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<List<Permiso>> GetListAsync()
        {
            var data = await _context.Permiso
                .Include(p => p.Perfiles) 
                .Where(p => p.IdEstado == 1)
                .Select(m => new Permiso
                {
                    PermisoId = m.PermisoId,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    Codigo = m.Codigo,
                    Perfiles = m.Perfiles.Select(p => new Perfil
                    {
                        PerfilId = p.PerfilId
                    }).ToList()
                })
                .ToListAsync();

            return data;
        }

    }
}
