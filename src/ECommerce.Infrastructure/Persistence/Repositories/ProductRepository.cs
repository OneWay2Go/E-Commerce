using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Database;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class ProductRepository(ECommerceDbContext context) : Repository<Product>(context), IProductRepository
{
}