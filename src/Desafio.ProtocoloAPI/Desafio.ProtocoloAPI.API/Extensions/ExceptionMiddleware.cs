using Desafio.ProtocoloAPI.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.API.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _log;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
    {
        _log = log;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode code = HttpStatusCode.InternalServerError;

        if (exception is NotFoundException) code = HttpStatusCode.NotFound;
        if (exception is ValidationException) code = HttpStatusCode.UnprocessableContent;

        string result = JsonSerializer.Serialize(new { error = exception.Message });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
