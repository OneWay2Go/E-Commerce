using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Persistence.Database;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Application.Models;
using ECommerce.Infrastructure.Auth.Seeders;
using ECommerce.Application.Mappers;
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

        // Non-Auth Entity Repositories
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IShippingAddressRepository, ShippingAddressRepository>();
        services.AddScoped<IWishListRepository, WishListRepository>();

        // Register Permission and RolePermission repositories
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

        // Register Mappers
        services.AddScoped<UserMapper>();
        services.AddScoped<RoleMapper>();
        services.AddScoped<UserRoleMapper>();
        services.AddScoped<PermissionMapper>();
        services.AddScoped<RolePermissionMapper>();
        services.AddScoped<CartMapper>();
        services.AddScoped<CartItemMapper>();
        services.AddScoped<CategoryMapper>();
        services.AddScoped<CouponMapper>();
        services.AddScoped<OrderMapper>();
        services.AddScoped<OrderItemMapper>();
        services.AddScoped<PaymentMapper>();
        services.AddScoped<ProductMapper>();
        services.AddScoped<ReviewMapper>();
        services.AddScoped<ShippingAddressMapper>();
        services.AddScoped<WishListMapper>();

        services.AddScoped<EmailService>();

        return services;
    }
}
