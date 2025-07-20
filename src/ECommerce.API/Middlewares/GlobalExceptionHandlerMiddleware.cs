using ECommerce.Application.Interfaces;
using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Log the full exception details
        _loggingService.LogError("Global exception handler caught: {ExceptionType} - {Message}", 
            exception.GetType().Name, exception.Message);
        _loggingService.LogError("Exception stack trace: {StackTrace}", exception.StackTrace);
        
        var response = new
        {
            Success = false,
            Message = "An unexpected error occurred.",
            Details = exception.Message
        };

        switch (exception)
        {
            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response = new { Success = false, Message = "Unauthorized access." };
                _loggingService.LogWarning("Unauthorized access attempt: {Path}", context.Request.Path);
                break;

            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new { Success = false, Message = "Invalid argument provided.", Details = exception.Message };
                _loggingService.LogWarning("Invalid argument: {Message}", exception.Message);
                break;

            case InvalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new { Success = false, Message = "Invalid operation.", Details = exception.Message };
                _loggingService.LogWarning("Invalid operation: {Message}", exception.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _loggingService.LogError("Unhandled exception occurred", exception);
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
} 