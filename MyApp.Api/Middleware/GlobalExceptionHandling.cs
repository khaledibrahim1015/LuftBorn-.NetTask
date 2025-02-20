using Microsoft.EntityFrameworkCore;
using MyApp.Application.Exceptions;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace MyApp.Api.Middleware;

public class GlobalExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandling> _logger;

    public GlobalExceptionHandling(RequestDelegate next, ILogger<GlobalExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Request received: {Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
            _logger.LogInformation("Request completed: {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught in GlobalExceptionHandling: {Message}", ex.Message);
            LogException(ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private void LogException(Exception ex)
    {
        var error = new
        {
            Message = ex.Message,
            StackTrace = ex.StackTrace,
            Source = ex.Source,
            InnerException = ex.InnerException?.Message,
            InnerExceptionStack = ex.InnerException?.StackTrace
        };

        _logger.LogError("Exception Details: {@ExceptionDetails}", error);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var error = new ApiErrorResponse();

        switch (exception)
        {
            case NotFoundException notFoundEx:
                error.StatusCode = HttpStatusCode.NotFound;
                error.Message = notFoundEx.Message;
                break;

            case ValidationException validationEx:
                error.StatusCode = HttpStatusCode.BadRequest;
                error.Message = validationEx.Message;
                break;

            case InvalidOperationException invOpEx:
                error.StatusCode = HttpStatusCode.BadRequest;
                error.Message = invOpEx.Message;
                break;

            case DbUpdateException dbEx:
                error.StatusCode = HttpStatusCode.InternalServerError;
                error.Message = "A database error occurred.";
                _logger.LogError(dbEx, "Database error: {Message}", dbEx.InnerException?.Message ?? dbEx.Message);
                break;

            default:
                error.StatusCode = HttpStatusCode.InternalServerError;
                error.Message = "An unexpected error occurred. Please try again later.";
                break;
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)error.StatusCode;

        var result = JsonSerializer.Serialize(error);
        await context.Response.WriteAsync(result);
    }
}

public class ApiErrorResponse
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }

    public ApiErrorResponse()
    {
        Errors = new Dictionary<string, string[]>();
    }
}