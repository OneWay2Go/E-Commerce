using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Interfaces;

public interface ILoggingService
{
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, Exception? exception = null, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogTrace(string message, params object[] args);
    void LogCritical(string message, Exception? exception = null, params object[] args);
    
    // Structured logging methods
    void LogUserAction(string action, int userId, string details = "");
    void LogApiCall(string endpoint, string method, int? userId = null, object? requestData = null);
    void LogDatabaseOperation(string operation, string entity, int? entityId = null);
    void LogAuthenticationEvent(string eventType, string email, bool success, string? reason = null);
    void LogPerformance(string operation, long elapsedMilliseconds);
} 