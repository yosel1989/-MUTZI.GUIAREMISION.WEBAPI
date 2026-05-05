using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class PerfilPermisoRepository: IPerfilPermisoRepository
    {
        private readonly LogisticaContext _context;
        public PerfilPermisoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task SyncAsync(List<PerfilPermiso> perfilPermisos)
        {
            // 1. Traer todos los registros actuales
            var actualesPorPermiso = await _context.PerfilPermiso
                .GroupBy(pp => pp.PermisoId)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            // 2. Agrupar lo recibido por permiso
            var recibidosPorPermiso = perfilPermisos
                .GroupBy(pp => pp.PermisoId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 3. Iterar sobre todos los permisos que existen en BD
            foreach (var kvp in actualesPorPermiso)
            {
                var permisoId = kvp.Key;
                var actuales = kvp.Value;

                if (recibidosPorPermiso.ContainsKey(permisoId))
                {
                    // Caso: el permiso existe en lo recibido
                    var nuevosIds = recibidosPorPermiso[permisoId]
                        .Select(pp => pp.PerfilId)
                        .Distinct()
                        .ToList();

                    if (!nuevosIds.Any())
                    {
                        // Si el array está vacío → eliminar todas las relaciones
                        _context.PerfilPermiso.RemoveRange(actuales);
                    }
                    else
                    {
                        // Eliminar los que ya no están
                        var eliminar = actuales
                            .Where(pp => !nuevosIds.Contains(pp.PerfilId))
                            .ToList();

                        // Agregar los nuevos que no existen
                        var existentesIds = actuales.Select(pp => pp.PerfilId).ToList();
                        var agregar = nuevosIds
                            .Where(id => !existentesIds.Contains(id))
                            .Select(id => new PerfilPermiso
                            {
                                PermisoId = permisoId,
                                PerfilId = id,
                                UsuarioRegistro = recibidosPorPermiso[permisoId].First().UsuarioRegistro,
                                FechaRegistro = DateTime.UtcNow
                            })
                            .ToList();

                        if (eliminar.Any())
                            _context.PerfilPermiso.RemoveRange(eliminar);

                        if (agregar.Any())
                            await _context.PerfilPermiso.AddRangeAsync(agregar);
                    }
                }
                else
                {
                    // Caso: el permiso NO existe en lo recibido → eliminar todas sus relaciones
                    _context.PerfilPermiso.RemoveRange(actuales);
                }
            }

            // 4. Permisos nuevos que no existían en BD
            var permisosNuevos = recibidosPorPermiso.Keys
                .Where(id => !actualesPorPermiso.ContainsKey(id))
                .ToList();

            foreach (var permisoId in permisosNuevos)
            {
                var nuevos = recibidosPorPermiso[permisoId];
                if (nuevos.Any())
                    await _context.PerfilPermiso.AddRangeAsync(nuevos);
            }

            await _context.SaveChangesAsync();
        }


    }
}
