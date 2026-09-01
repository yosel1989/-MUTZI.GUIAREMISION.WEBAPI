using FluentValidation;
using FluentValidation.Results;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using System.Text.Json;

namespace MUTZI.GUIA.WEBAPI.Middlewares
{
    public class FluentValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public FluentValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
            {

                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(body))
                {

                    var dto = JsonSerializer.Deserialize<EntityCreateDTO>(body);

                    if (dto != null)
                    {
                        var validator = context.RequestServices.GetService<IValidator<EntityCreateDTO>>();
                        if (validator != null)
                        {
                            ValidationResult result = await validator.ValidateAsync(dto);
                            if (!result.IsValid)
                            {
                                var errors = result.Errors.Select(e => new
                                {
                                    Field = e.PropertyName,
                                    Message = e.ErrorMessage
                                });

                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                                {
                                    status = 1001,
                                    error = "Error de validación.",
                                    detalle = string.Join(" | ", errors.Select(e => e.Message))
                                }));
                                return;
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
