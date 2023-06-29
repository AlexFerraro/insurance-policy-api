using insurance_policy_api_domain.Exceptions;
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
                when (ex is PolicyNotFoundException
                                || ex is InstallmentNotFoundException
                                || ex is PaymentAlreadyMadeException)
        {
            LogBusinessException(httpContext, ex);
            await HandleBusinessExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            LogInfrastructureException(httpContext, ex);
            await HandleInfrastructureExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleBusinessExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = GetBusinessExceptionDetails(exception);

        await WriteJsonResponseAsync(context, statusCode, message);
    }

    private async Task HandleInfrastructureExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = GetInfrastructureExceptionDetails(exception);

        await WriteJsonResponseAsync(context, statusCode, message);
    }

    private (HttpStatusCode statusCode, string message) GetBusinessExceptionDetails(Exception exception)
    {
        var (statusCode, message) = (HttpStatusCode.InternalServerError, string.Empty);

        try
        {
            (statusCode, message) = exception switch
            {
                PaymentAlreadyMadeException => (HttpStatusCode.Conflict, exception.Message),

                InstallmentNotFoundException => (HttpStatusCode.NotFound, exception.Message),

                PolicyNotFoundException => (HttpStatusCode.NotFound, exception.Message),

                _ => throw new NotImplementedException()
            };
        }
        catch (NotImplementedException nie) //Nie is a shortened form of NotImplementedException.
        {
            LogCriticalBusinessException(nie);

            /*var errorLog = new
            {
                ExceptionType = nie.GetType().FullName,
                ExceptionMessage = nie.Message,
                StackTrace = nie.StackTrace,
                InnerExceptionType = nie.InnerException?.GetType().FullName,
                InnerExceptionMessage = nie.InnerException?.Message,
                InnerExceptionStackTrace = nie.InnerException?.StackTrace,
                Timestamp = DateTime.UtcNow
            };

            // Store the errorLog object in the NoSQL database
            await NoSqlDatabase.SaveErrorLogAsync(errorLog);*/
        }

        return (statusCode, message);
    }

    private (HttpStatusCode statusCode, string message) GetInfrastructureExceptionDetails(Exception exception)
    {
        return exception switch
        {
            DbUpdateException { InnerException: NpgsqlException { InnerException: TimeoutException } } =>
                (HttpStatusCode.RequestTimeout, "The request could not be processed due to a timeout in the database connection."),

            DbUpdateException { InnerException: NpgsqlException } =>
                (HttpStatusCode.InternalServerError, "The request could not be processed due to a database error."),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please contact support.")
        };
    }

    private async Task WriteJsonResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = context.Request.ContentType;

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private void LogBusinessException(HttpContext context, Exception exception) =>
        _logger.LogInformation(exception, "An non-critical exception occurred in the business model. Exception Details => Request TraceIdentifier: {TraceIdentifier}, ExceptionType: {ExceptionType}, Reason: {ExceptionMessage}",
            context.TraceIdentifier, exception.GetType().FullName, exception.Message);

    private void LogCriticalBusinessException(NotImplementedException exception) =>
        _logger.LogCritical(exception, "An unhandled critical business exception occurred. Exception Details => Exception Type: {ExceptionType}, Exception Message: {ExceptionMessage}, StackTrace : {StackTrace}",
            exception.GetType().FullName, exception.Message, exception.StackTrace);

    private void LogInfrastructureException(HttpContext context, Exception exception) =>
        _logger.LogError(exception, "An exception occurred during request execution. Exception Details => Request TraceIdentifier: {TraceIdentifier}, ExceptionType: {ExceptionType}, Reason: {ExceptionMessage}, StackTrace: {StackTrace}",
            context.TraceIdentifier, exception.GetType().FullName, exception.Message, exception.StackTrace);
}