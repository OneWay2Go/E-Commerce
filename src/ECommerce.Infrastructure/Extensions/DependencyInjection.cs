using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Persistence.Database;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Application.Models;
using ECommerce.Infrastructure.Auth.Seeders;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ECommerceDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped(typeof(IUserRoleRepository), typeof(UserRoleRepository));

        services.AddScoped<IEmailRepository, EmailRepository>();

        services.AddScoped<EmailService>();

        return services;
    }
}
