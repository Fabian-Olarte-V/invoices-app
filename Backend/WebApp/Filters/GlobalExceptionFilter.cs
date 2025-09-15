using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WebApp.Models.Response;

namespace WebApp.Filters;

public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);

        var response = context.Exception switch
        {
            ValidationException validationEx => new ValidationErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Error de validación",
                Detail = "Se encontraron uno o más errores de validación",
                Errors = validationEx.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray())
            },
            EntityNotFoundException ex => new ErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Title = "Entidad no encontrada",
                Detail = ex.Message
            },
            BusinessRuleValidationException ex => new ErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Error de validación",
                Detail = ex.Message
            },
            UniqueConstraintException ex => new ValidationErrorResponse
            {
                Status = HttpStatusCode.Conflict,
                Title = "Error de duplicidad",
                Detail = ex.Message,
                Errors = new Dictionary<string, string[]>
                {
                    { ex.Field, new[] { ex.Message } }
                }
            },
            ForeingKeyConstraintException ex => new ValidationErrorResponse
            {
                Status = HttpStatusCode.Conflict,
                Title = "Error de relacion",
                Detail = ex.Message,
                Errors = new Dictionary<string, string[]>
                {
                    { ex.Field, new[] { ex.Message } }
                }
            },
            _ => new ErrorResponse
            {
                Status = HttpStatusCode.InternalServerError,
                Title = "Se produjo un error interno",
                Detail = context.Exception.Message
            }
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = (int)response.Status
        };

        base.OnException(context);
    }
}