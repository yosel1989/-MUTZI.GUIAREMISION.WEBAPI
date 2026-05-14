using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class GuiaRemisionEstadoPermisoRepository: IGuiaRemisionEstadoPermisoRepository
    {
        private readonly LogisticaContext _context;
        public GuiaRemisionEstadoPermisoRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task SyncAsync(List<GuiaRemisionEstadoPermiso> permisos)
        {
            // 1. Traer todos los registros actuales
            var actualesPorPermiso = await _context.GuiaRemisionEstadoPermiso
                .GroupBy(pp => pp.GuiaRemisionEstadoId)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            // 2. Agrupar lo recibido por permiso
            var recibidosPorPermiso = permisos
                .GroupBy(pp => pp.GuiaRemisionEstadoId)
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
                        .Select(pp => pp.GuiaRemisionEstadoIdPermitido)
                        .Distinct()
                        .ToList();

                    if (!nuevosIds.Any())
                    {
                        // Si el array está vacío → eliminar todas las relaciones
                        _context.GuiaRemisionEstadoPermiso.RemoveRange(actuales);
                    }
                    else
                    {
                        // Eliminar los que ya no están
                        var eliminar = actuales
                            .Where(pp => !nuevosIds.Contains(pp.GuiaRemisionEstadoIdPermitido))
                            .ToList();

                        // Agregar los nuevos que no existen
                        var existentesIds = actuales.Select(pp => pp.GuiaRemisionEstadoIdPermitido).ToList();
                        var agregar = nuevosIds
                            .Where(id => !existentesIds.Contains(id))
                            .Select(id => new GuiaRemisionEstadoPermiso
                            {
                                GuiaRemisionEstadoId = permisoId,
                                GuiaRemisionEstadoIdPermitido = id,
                                UsuarioRegistro = recibidosPorPermiso[permisoId].First().UsuarioRegistro,
                                UsuarioRegistroNombre = recibidosPorPermiso[permisoId].First().UsuarioRegistroNombre,
                                FechaRegistro = DateTime.UtcNow
                            })
                            .ToList();

                        if (eliminar.Any())
                            _context.GuiaRemisionEstadoPermiso.RemoveRange(eliminar);

                        if (agregar.Any())
                            await _context.GuiaRemisionEstadoPermiso.AddRangeAsync(agregar);
                    }
                }
                else
                {
                    // Caso: el permiso NO existe en lo recibido → eliminar todas sus relaciones
                    _context.GuiaRemisionEstadoPermiso.RemoveRange(actuales);
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
                    await _context.GuiaRemisionEstadoPermiso.AddRangeAsync(nuevos);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GuiaRemisionEstadoPermiso>> GetByEstado(int guiaRemisionEstadoId)
        {
            try
            {
                var permisos = await _context.GuiaRemisionEstadoPermiso
                    .Where(p => p.GuiaRemisionEstadoId == guiaRemisionEstadoId)
                    .ToListAsync();
                return permisos;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
