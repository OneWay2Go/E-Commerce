using ECommerce.Application.Interfaces;
using System.Diagnostics;

namespace ECommerce.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static async Task<T?> GetByIdWithLoggingAsync<T>(this IRepository<T> repository, int id, ILoggingService loggingService) where T : class
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            loggingService.LogDebug("Fetching {EntityType} with ID: {Id}", typeof(T).Name, id);
            var result = await repository.GetByIdAsync(id);
            stopwatch.Stop();
            
            if (result != null)
            {
                loggingService.LogInformation("Successfully fetched {EntityType} with ID: {Id} in {ElapsedMs}ms", 
                    typeof(T).Name, id, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                loggingService.LogWarning("{EntityType} with ID: {Id} not found", typeof(T).Name, id);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            loggingService.LogError("Error fetching {EntityType} with ID: {Id}", ex, typeof(T).Name, id);
            throw;
        }
    }

    public static async Task<T> AddWithLoggingAsync<T>(this IRepository<T> repository, T entity, ILoggingService loggingService) where T : class
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            loggingService.LogDebug("Adding {EntityType}", typeof(T).Name);
            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            stopwatch.Stop();
            
            loggingService.LogInformation("Successfully added {EntityType} in {ElapsedMs}ms", 
                typeof(T).Name, stopwatch.ElapsedMilliseconds);
            loggingService.LogDatabaseOperation("Insert", typeof(T).Name);
            
            return entity;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            loggingService.LogError("Error adding {EntityType}", ex, typeof(T).Name);
            throw;
        }
    }

    public static async Task UpdateWithLoggingAsync<T>(this IRepository<T> repository, T entity, ILoggingService loggingService) where T : class
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            loggingService.LogDebug("Updating {EntityType}", typeof(T).Name);
            repository.Update(entity);
            await repository.SaveChangesAsync();
            stopwatch.Stop();
            
            loggingService.LogInformation("Successfully updated {EntityType} in {ElapsedMs}ms", 
                typeof(T).Name, stopwatch.ElapsedMilliseconds);
            loggingService.LogDatabaseOperation("Update", typeof(T).Name);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            loggingService.LogError("Error updating {EntityType}", ex, typeof(T).Name);
            throw;
        }
    }

    public static async Task DeleteWithLoggingAsync<T>(this IRepository<T> repository, T entity, ILoggingService loggingService) where T : class
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            loggingService.LogDebug("Deleting {EntityType}", typeof(T).Name);
            repository.Update(entity);
            await repository.SaveChangesAsync();
            stopwatch.Stop();
            
            loggingService.LogInformation("Successfully deleted {EntityType} in {ElapsedMs}ms", 
                typeof(T).Name, stopwatch.ElapsedMilliseconds);
            loggingService.LogDatabaseOperation("Delete", typeof(T).Name);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            loggingService.LogError("Error deleting {EntityType}", ex, typeof(T).Name);
            throw;
        }
    }

    public static IQueryable<T> GetByConditionWithLogging<T>(this IRepository<T> repository, 
        System.Linq.Expressions.Expression<Func<T, bool>> condition, 
        ILoggingService loggingService) where T : class
    {
        try
        {
            loggingService.LogDebug("Querying {EntityType} with condition", typeof(T).Name);
            var result = repository.GetByCondition(condition);
            loggingService.LogInformation("Successfully queried {EntityType} with condition", typeof(T).Name);
            return result;
        }
        catch (Exception ex)
        {
            loggingService.LogError("Error querying {EntityType} with condition", ex, typeof(T).Name);
            throw;
        }
    }
} 