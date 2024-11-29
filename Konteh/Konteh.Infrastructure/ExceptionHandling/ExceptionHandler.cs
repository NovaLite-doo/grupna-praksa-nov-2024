using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.Infrastructure.ExceptionHandling
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                await HandleValidationException(httpContext, validationException, cancellationToken);
                return true;
            }
            else if (exception is EntityNotFoundException entityNotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return true;
            }
            return false;
        }

        private static async Task HandleValidationException(HttpContext httpContext, ValidationException exception, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();

            var properties = exception.Errors.Select(x => x.PropertyName).Distinct();
            foreach (var property in properties)
            {
                errors[property] = exception.Errors
                    .Where(x => x.PropertyName == property)
                    .Select(x => x.ErrorMessage)
                    .ToArray();
            }

            ValidationProblemDetails problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request"
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
    }
}
