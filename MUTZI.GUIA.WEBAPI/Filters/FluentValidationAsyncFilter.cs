using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MUTZI.GUIA.WEBAPI.Filters
{
    public class FluentValidationAsyncFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dto = context.ActionArguments.Values.FirstOrDefault();

            if (dto == null)
            {
                await next();
                return;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(dto.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType);

            if (validator != null)
            {
                var method = validatorType.GetMethod("ValidateAsync", new[] { dto.GetType(), typeof(CancellationToken) });
                var task = (Task<FluentValidation.Results.ValidationResult>)method.Invoke(validator, new object[] { dto, CancellationToken.None });
                var result = await task;

                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(err => new
                    {
                        Field = err.PropertyName,
                        Message = err.ErrorMessage
                    }).ToList();

                    context.Result = new BadRequestObjectResult(new 
                    {
                        Status = 1001,
                        detalle = errors.Select(e => e.Message).FirstOrDefault() ?? ""
                    });

                    return;
                }
            }

            await next();
        }
    }
}
