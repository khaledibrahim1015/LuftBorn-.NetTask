using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;
using MyApp.Infrastructure.Data;
using System.Linq.Expressions;

namespace MyApp.Infrastructure.Repositories;

public class RepositoryBase<TEntity, TKey> : IAsyncBaseRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
            => _context = context;


    public async Task<IQueryable<TEntity>> GetAllAsync()
       => await Task.FromResult(_context.Set<TEntity>().AsQueryable());

    public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        => await Task.FromResult(_context.Set<TEntity>().Where(predicate).AsQueryable());

    public async Task<TEntity> GetByIdAsync(TKey id)
        => await _context.Set<TEntity>().FindAsync(id);

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        // Ensure the entity is being tracked
        var existingEntity = await _context.Set<TEntity>().FindAsync(entity.Id);
        if (existingEntity != null)
        {
            // Update the existing entity's properties with the values from the detached entity
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteAsync(TKey id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public IQueryable<TEntity> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        => _context.Set<TEntity>().Where(predicate);

}
