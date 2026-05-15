using MUTZI.GUIA.APPLICATION.DTOs.Efact;

namespace MUTZI.GUIA.WEBAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Operación inválida");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new BadRequestResponseDTO
                {
                    Error = "Operación inválida",
                    Detalle = ex.Message
                });
            }
            /*catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "No encontrado");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new BadRequestResponseDTO
                {
                    Error = "Recurso no encontrado",
                    Detalle = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new BadRequestResponseDTO
                {
                    Error = "Error interno",
                    Detalle = ex.Message
                });
            }*/
        }
    }

}
