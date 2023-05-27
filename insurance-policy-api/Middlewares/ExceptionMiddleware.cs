using insurance_policy_api_domain.Excepitions;
using Microsoft.AspNetCore.Http;
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
            _logger.LogInformation(ex, "An non-critical exception occurred in the business model. Exception Details => Request TraceIdentifier: {TraceIdentifier}, ExceptionType: {ExceptionType}, Reason: {ExceptionMessage}"
                        , httpContext.TraceIdentifier, ex.GetType().FullName, ex.Message);

            await HandleBusinessExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex
                        , "An exception occurred during request execution. Exception Details => Request TraceIdentifier: {TraceIdentifier}, ExceptionType: {ExceptionType}, Reason: {ExceptionMessage}, StackTrace: {StackTrace}"
                        , httpContext.TraceIdentifier, ex.GetType().FullName, ex.Message, ex.StackTrace);
            
            await HandleInfrastructureExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleInfrastructureExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            DbUpdateException { InnerException: NpgsqlException { InnerException: TimeoutException } } =>
                (HttpStatusCode.RequestTimeout, "The request could not be processed due to a timeout in the database connection."),

            DbUpdateException { InnerException: NpgsqlException } =>
                (HttpStatusCode.InternalServerError, "The request could not be processed due to a database error."),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please contact support.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = context.Request.ContentType;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = (int)statusCode,
            Message = message
        }));
    }
    
    private async Task HandleBusinessExceptionAsync(HttpContext context, Exception exception)
    {
        var(statusCode, message) = ( HttpStatusCode.InternalServerError, "" ); //ValueTuple here.

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
            _logger.LogCritical(nie, "An unhandled critical business exception occurred. Exception Details => Exception Type: {ExceptionType}, Exception Message: {ExceptionMessage}, StackTrace : {StackTrace}"
                        , nie.GetType().FullName, nie.Message, nie.StackTrace);

            var errorLog = new
            {
                ExceptionType = nie.GetType().FullName,
                ExceptionMessage = nie.Message,
                StackTrace = nie.StackTrace,
                InnerExceptionType = nie.InnerException?.GetType().FullName,
                InnerExceptionMessage = nie.InnerException?.Message,
                InnerExceptionStackTrace = nie.InnerException?.StackTrace,
                Timestamp = DateTime.UtcNow
            };

            // Store the errorLog object in the NoSQL database - Please don`t use await here!
            // var saveErrorLogTask = Task.Run(async() => await NoSqlDatabase.SaveErrorLogAsync(errorLog));
            // /*await*/ saveErrorLogTask; Never do that.
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = context.Request.ContentType;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = (int)statusCode,
            Message = message
        }));
    }
}//////Terminar retorno do response para aceitar json, xml e html na chamada.