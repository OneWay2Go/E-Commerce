using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Database;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class ReviewRepository(ECommerceDbContext context) : Repository<Review>(context), IReviewRepository
{
}