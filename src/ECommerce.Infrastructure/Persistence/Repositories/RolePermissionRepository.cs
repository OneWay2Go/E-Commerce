using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Auth;
using ECommerce.Infrastructure.Persistence.Database;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class RolePermissionRepository(ECommerceDbContext context) : Repository<RolePermission>(context), IRolePermissionRepository
{
} 