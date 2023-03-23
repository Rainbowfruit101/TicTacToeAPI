using Microsoft.EntityFrameworkCore;

namespace Database;

public interface IRepository<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    TDbContext DbContext { get; }
    DbSet<TEntity> GetDbSet();
    IQueryable<TEntity> GetQueryable();
}