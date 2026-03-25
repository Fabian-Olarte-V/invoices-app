using Application.Common.Exceptions;
using Domain.Exceptions;
using Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;
using WebApp.Models.Response;

namespace WebApp.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            ApplicationValidationException validationEx => new ValidationErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Error de validacion",
                Detail = validationEx.Message,
                Errors = validationEx.Errors
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
                Title = "Error de validacion",
                Detail = ex.Message
            },
            UniqueConstraintException ex => new ValidationErrorResponse
            {
                Status = HttpStatusCode.Conflict,
                Title = "Error de duplicidad",
                Detail = ex.Message,
                Errors = new Dictionary<string, string[]>
                {
                    [ex.Field] = new[] { ex.Message }
                }
            },
            ForeignKeyConstraintException ex => new ValidationErrorResponse
            {
                Status = HttpStatusCode.Conflict,
                Title = "Error de relacion",
                Detail = ex.Message,
                Errors = new Dictionary<string, string[]>
                {
                    [ex.Field] = new[] { ex.Message }
                }
            },
            _ => new ErrorResponse
            {
                Status = HttpStatusCode.InternalServerError,
                Title = "Se produjo un error interno",
                Detail = exception.Message
            }
        };

        context.Response.StatusCode = (int)response.Status;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
