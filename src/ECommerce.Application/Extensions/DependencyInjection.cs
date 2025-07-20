using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Mappers
        services.AddScoped<UserMapper>();
        services.AddScoped<ProfileMapper>();
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


        return services;
    }
}