using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;
using MUTZI.GUIA.WEBAPI.Attributes;

namespace MUTZI.GUIA.WEBAPI.Filters
{
    public class PermissionFilter : IAsyncActionFilter
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly LogisticaContext _context;

        public PermissionFilter(IAuthService authService, IConfiguration configuration, LogisticaContext context)
        {
            _authService = authService;
            _configuration = configuration;
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            

            var permissionAttr = context.ActionDescriptor.EndpointMetadata
                .OfType<PermissionAttribute>()
                .FirstOrDefault();

            if (permissionAttr == null)
            {
                await next();
                return;
            }

            var user = _authService.CurrentUser();
            var appId = _configuration.GetValue<int>("App:Id");

            var codigosPerfilesFiltrados = user.Profiles
                .Where(p => p.IdApp == appId)
                .Select(p => p.Id)
                .ToList();

            if (permissionAttr != null)
            {
                // 3. Validar en BD si alguno de los perfiles filtrados tiene el permiso con ese código
                var tienePermiso = await _context.PerfilPermiso
                    .Include(pp => pp.Permiso)
                    .Include(pp => pp.Perfil)
                    .AnyAsync(pp => codigosPerfilesFiltrados.Contains(pp.Perfil.Codigo) && // comparar contra la columna Codigo
                                    pp.Permiso.Codigo == permissionAttr.Codigo);

                if (!tienePermiso)
                {
                    var errorResponse = new BadRequestResponseDTO
                    {
                        Error = "Error de autorización.",
                        Detalle = $"No tiene permiso para la acción: {permissionAttr.Codigo}"
                    };

                    context.Result = new BadRequestObjectResult(errorResponse);
                    return;
                }
            }

            await next();
        }


    }
}
