using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ECommerce.Infrastructure.Auth.Seeders;

public class PermissionSeeder(ECommerceDbContext context)
{
    public async Task SeedAsync()
    {
        var enumNames = Enum.GetNames(typeof(Domain.Enums.Permission)).ToHashSet();

        var existingPermissions = await context.Permissions.ToListAsync();

        var toAdd = enumNames
            .Where(name => existingPermissions.All(p => p.Name != name))
            .Select(name => new Permission
            {
                Name = name,
                IsDeleted = false
            }).ToList();

        var toRemove = existingPermissions
            .Where(p => !enumNames.Contains(p.Name))
            .ToList();

        if (toAdd.Any())
        {
            context.Permissions.AddRange(toAdd);
            Log.Information($"{toAdd.Count} permissions added.");
        }

        if (toRemove.Any())
        {
            context.Permissions.RemoveRange(toRemove); 
            Log.Information($"{toRemove.Count} permissions removed.");
        }

        if (toAdd.Any() || toRemove.Any())
            await context.SaveChangesAsync();
        else
            Log.Information("Permissions are already in sync.");
    }
}
