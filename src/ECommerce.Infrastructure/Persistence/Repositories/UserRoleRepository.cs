using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Database;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class UserRoleRepository(ECommerceDbContext context) : Repository<UserRole>(context), IUserRoleRepository
{
}
