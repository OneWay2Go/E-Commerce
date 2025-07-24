using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Auth;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Database;

public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Coupon> Coupons { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<ShippingAddress> ShippingAddresses { get; set; }

    public DbSet<WishList> WishLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var salt = Guid.NewGuid().ToString();
        var passwordHasher = new PasswordHasher();
        var hashedPassword = passwordHasher.Encrypt("adminadmin", salt);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@example.com",
                FullName = "Admin User",
                IsDeleted = false,
                //IsEmailConfirmed = true,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                PhoneNumber = "901101613",
                //Code = Guid.NewGuid().ToString()
            }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "Admin",
                IsDeleted = false
            },
            new Role
            {
                Id = 2,
                Name = "User",
                IsDeleted = false
            }
        );

        modelBuilder.Entity<Permission>().HasData
        (
            new Permission
            {
                Id = 1,
                Name = "AdminPermission",
                IsDeleted = false
            }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                Id = 1,
                UserId = 1,
                RoleId = 1
            }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission
            {
                Id = 1,
                RoleId = 1,
                PermissionId = 1
            }
        );
    }
}
