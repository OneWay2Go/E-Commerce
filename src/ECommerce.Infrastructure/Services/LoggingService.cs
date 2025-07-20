using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ECommerce.Infrastructure.Services;

public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string message, Exception? exception = null, params object[] args)
    {
        if (exception != null)
            _logger.LogError(exception, message, args);
        else
            _logger.LogError(message, args);
    }

    public void LogDebug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }

    public void LogTrace(string message, params object[] args)
    {
        _logger.LogTrace(message, args);
    }

    public void LogCritical(string message, Exception? exception = null, params object[] args)
    {
        if (exception != null)
            _logger.LogCritical(exception, message, args);
        else
            _logger.LogCritical(message, args);
    }

    public void LogUserAction(string action, int userId, string details = "")
    {
        _logger.LogInformation("User Action: {Action} by User {UserId}. Details: {Details}", 
            action, userId, details);
    }

    public void LogApiCall(string endpoint, string method, int? userId = null, object? requestData = null)
    {
        _logger.LogInformation("API Call: {Method} {Endpoint} by User {UserId}. Request: {@RequestData}", 
            method, endpoint, userId, requestData);
    }

    public void LogDatabaseOperation(string operation, string entity, int? entityId = null)
    {
        _logger.LogInformation("Database Operation: {Operation} on {Entity} with ID {EntityId}", 
            operation, entity, entityId);
    }

    public void LogAuthenticationEvent(string eventType, string email, bool success, string? reason = null)
    {
        var logLevel = success ? LogLevel.Information : LogLevel.Warning;
        _logger.Log(logLevel, "Authentication Event: {EventType} for {Email} - Success: {Success}. Reason: {Reason}", 
            eventType, email, success, reason ?? "N/A");
    }

    public void LogPerformance(string operation, long elapsedMilliseconds)
    {
        var logLevel = elapsedMilliseconds > 1000 ? LogLevel.Warning : LogLevel.Information;
        _logger.Log(logLevel, "Performance: {Operation} took {ElapsedMs}ms", 
            operation, elapsedMilliseconds);
    }
} 