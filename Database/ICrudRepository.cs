using Microsoft.EntityFrameworkCore;

namespace Database;

public interface ICrudRepository<TDbContext, TEntity> : IRepository <TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    Task<IEnumerable<TEntity>> ReadAllAsync();
    Task<TEntity> CreateAsync(Func<Guid, TEntity?> entityProducer);
    Task<TEntity> ReadAsync(Guid id);
    Task<TEntity> UpdateAsync(TEntity source);
    Task<TEntity> DeleteAsync(Guid id);
}