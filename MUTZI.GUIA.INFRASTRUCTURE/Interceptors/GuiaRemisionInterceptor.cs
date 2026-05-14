using DocumentFormat.OpenXml.InkML;
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
            var cambios = context.ChangeTracker.Entries<GuiaRemision>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in cambios)
            {
                var original = (int)entry.OriginalValues["IdEstado"];
                var nuevo = (int)entry.CurrentValues["IdEstado"];

                var permisos = await context.GuiaRemisionEstadoPermiso
                    .Where(p => p.GuiaRemisionEstadoId == nuevo)
                    .ToListAsync(cancellationToken);

                if (permisos.Any() && !permisos.Any(p => p.GuiaRemisionEstadoIdPermitido == original))
                {
                    // Obtener nombres de estados
                    var estadoOriginal = await context.GuiaRemisionEstado
                        .Where(e => e.GuiaRemisionEstadoId == original)
                        .Select(e => e.Nombre)
                        .FirstOrDefaultAsync(cancellationToken);

                    var estadoNuevo = await context.GuiaRemisionEstado
                        .Where(e => e.GuiaRemisionEstadoId == nuevo)
                        .Select(e => e.Nombre)
                        .FirstOrDefaultAsync(cancellationToken);

                    throw new InvalidOperationException(
                        $"Operación inválida, cambio de estado '{estadoOriginal}' a '{estadoNuevo}'");
                }
            }

            // Aquí puedes validar cambios directamente con el mismo contexto
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }

    /*public class GuiaRemisionInterceptor : SaveChangesInterceptor
    {
        private readonly IDbContextFactory<LogisticaContext> _contextFactory;

        public GuiaRemisionInterceptor(IDbContextFactory<LogisticaContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var cambios = eventData.Context.ChangeTracker.Entries<GuiaRemision>()
                .Where(e => e.State == EntityState.Modified);

            using var consultaContext = _contextFactory.CreateDbContext();

            foreach (var entry in cambios)
            {
                var original = (int)entry.OriginalValues["IdEstado"];
                var nuevo = (int)entry.CurrentValues["IdEstado"];

                var permisos = await consultaContext.GuiaRemisionEstadoPermiso
                    .Where(p => p.GuiaRemisionEstadoId == nuevo)
                    .ToListAsync(cancellationToken);

                if (permisos.Any() && !permisos.Any(p => p.GuiaRemisionEstadoIdPermitido == original))
                {
                    throw new InvalidOperationException(
                        $"Transición inválida de estado {original} a {nuevo}");
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }

    */
}
