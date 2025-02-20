using MyApp.Core.Entitties;
using System.Linq.Expressions;

namespace MyApp.Core.Inrerfaces;

public interface IAsyncBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<IQueryable<TEntity>> GetAllAsync();
    Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetByIdAsync(TKey id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);
    IQueryable<TEntity> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
}
