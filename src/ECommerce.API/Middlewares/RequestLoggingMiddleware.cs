using ECommerce.Application.Interfaces;
using System.Diagnostics;
using System.Text;

namespace ECommerce.API.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public RequestLoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            // Log the incoming request
            var requestInfo = await LogRequest(context);
            
            // Capture response body
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Process the request
            await _next(context);

            stopwatch.Stop();

            // Log the response
            await LogResponse(context, memoryStream, stopwatch.ElapsedMilliseconds, requestInfo);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _loggingService.LogError("Request processing failed", ex);
            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<string> LogRequest(HttpContext context)
    {
        var request = context.Request;
        var requestInfo = $"HTTP {request.Method} {request.Path}{request.QueryString}";
        
        // Get user ID if authenticated
        var userId = GetUserIdFromContext(context);
        
        // Log request details
        _loggingService.LogApiCall(
            $"{request.Path}{request.QueryString}", 
            request.Method, 
            userId,
            new
            {
                UserAgent = request.Headers["User-Agent"].ToString(),
                ContentType = request.ContentType,
                ContentLength = request.ContentLength,
                Host = request.Host.ToString(),
                Protocol = request.Protocol
            });

        return requestInfo;
    }

    private async Task LogResponse(HttpContext context, MemoryStream responseBody, long elapsedMs, string requestInfo)
    {
        var response = context.Response;
        
        // Get response body
        responseBody.Position = 0;
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Position = 0;
        await responseBody.CopyToAsync(context.Response.Body);

        var userId = GetUserIdFromContext(context);
        
        // Log response details
        _loggingService.LogInformation(
            "Response: {RequestInfo} -> {StatusCode} ({ElapsedMs}ms) by User {UserId}",
            requestInfo, response.StatusCode, elapsedMs, userId);

        // Log performance if slow
        if (elapsedMs > 1000)
        {
            _loggingService.LogPerformance($"{requestInfo}", elapsedMs);
        }

        // Log errors
        if (response.StatusCode >= 400)
        {
            _loggingService.LogWarning(
                "Error Response: {RequestInfo} -> {StatusCode} by User {UserId}. Response: {ResponseBody}",
                requestInfo, response.StatusCode, userId, responseText);
        }
    }

    private int? GetUserIdFromContext(HttpContext context)
    {
        try
        {
            var userIdClaim = context.User?.Claims?.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;
        }
        catch
        {
            // Ignore errors in user ID extraction
        }
        return null;
    }
} 