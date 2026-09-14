using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Interceptors
{
    public class GuiaRemisionInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context as LogisticaContext;

            // Solo entradas modificadas donde realmente cambió el EstadoId
            var cambios = context.ChangeTracker.Entries<GuiaRemision>()
                .Where(e => e.State == EntityState.Modified &&
                            !Equals(e.OriginalValues["EstadoId"], e.CurrentValues["EstadoId"]));

            foreach (var entry in cambios)
            {
                var original = (int)entry.OriginalValues["EstadoId"];
                var nuevo = (int)entry.CurrentValues["EstadoId"];

                // Validar si existe permiso directo para la transición
                var existePermiso = await context.GuiaRemisionEstadoPermiso
                    .AnyAsync(p => p.GuiaRemisionEstadoId == nuevo &&
                                   p.GuiaRemisionEstadoIdPermitido == original,
                              cancellationToken);

                if (!existePermiso)
                {
                    // Obtener nombres de ambos estados en una sola consulta
                    var estados = await context.GuiaRemisionEstado
                        .Where(e => e.GuiaRemisionEstadoId == original || e.GuiaRemisionEstadoId == nuevo)
                        .ToDictionaryAsync(e => e.GuiaRemisionEstadoId, e => e.Nombre, cancellationToken);

                    var estadoOriginal = estados.ContainsKey(original) ? estados[original] : $"Id:{original}";
                    var estadoNuevo = estados.ContainsKey(nuevo) ? estados[nuevo] : $"Id:{nuevo}";

                    throw new InvalidOperationException(
                        $"Operación inválida, cambio de estado '{estadoOriginal}' a '{estadoNuevo}'");
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
