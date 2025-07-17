using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces;

public interface IRepository<TEntity>
{
    Task AddAsync(TEntity entity);
    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(int id);
    TEntity? GetById(int id);

    IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate);

    Task<int> SaveChangesAsync();
    int SaveChanges();
}
