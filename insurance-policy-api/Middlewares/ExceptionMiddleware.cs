using insurance_policy_api_domain.Excepitions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Text.Json;

namespace insurance_policy_api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção durante a execução da request");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            DbUpdateException { InnerException: NpgsqlException { InnerException: TimeoutException } } =>
                (HttpStatusCode.RequestTimeout, "The request could not be processed due to a timeout in the database connection."),

            DbUpdateException { InnerException: NpgsqlException } =>
                (HttpStatusCode.InternalServerError, "The request could not be processed due to a database error."),

            PolicyNotFoundException => (HttpStatusCode.NotFound, exception.Message),

            InstallmentNotFoundException => (HttpStatusCode.NotFound, exception.Message),

            PaymentAlreadyMadeException => (HttpStatusCode.Conflict, exception.Message),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please contact support.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = (int)statusCode,
            Message = message
        }));
    }
}
