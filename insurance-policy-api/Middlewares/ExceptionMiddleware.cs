using insurance_policy_api.DTOs;
using Microsoft.EntityFrameworkCore;
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
            _logger.LogError(ex, "Ocorreu uma exceção não tratada.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string message;

        if (exception is DbUpdateException)
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = "A solicitação não pôde ser processada devido a um erro no banco de dados.";
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = "Ocorreu um erro inesperado, entre em contato com o suporte.";
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = (int)statusCode,
            Message = message
        }));
    }
}
