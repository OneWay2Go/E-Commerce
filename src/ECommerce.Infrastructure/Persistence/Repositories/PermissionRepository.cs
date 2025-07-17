using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities.Auth;
using ECommerce.Infrastructure.Persistence.Database;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class PermissionRepository(ECommerceDbContext context) : Repository<Permission>(context), IPermissionRepository
{
} 