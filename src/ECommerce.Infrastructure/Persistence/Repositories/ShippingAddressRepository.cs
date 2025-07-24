using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Database;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class ShippingAddressRepository(ECommerceDbContext context) : Repository<ShippingAddress>(context), IShippingAddressRepository
{
}