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
            _logger.LogError(ex, "Ocorreu uma exceção não tratada.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            DbUpdateException { InnerException: NpgsqlException { InnerException: TimeoutException } } =>
                (HttpStatusCode.RequestTimeout, "A solicitação não pôde ser processada devido ao timeout na conexão com o banco de dados."),

            DbUpdateException { InnerException: NpgsqlException } =>
                (HttpStatusCode.InternalServerError, "A solicitação não pôde ser processada devido a um erro no banco de dados."),
            
            NotFoundException { InnerException: InvalidOperationException } =>
                            (HttpStatusCode.BadRequest, "A solicitação não pode ser processada devido que uma das parcelas da apólice informada não existe na base de dados."),
            
            NotFoundException => (HttpStatusCode.BadRequest, $"A solicitação não pode ser processada devido ao seguinte ocorrido: { exception.Message }"),

            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado, entre em contato com o suporte.")
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
