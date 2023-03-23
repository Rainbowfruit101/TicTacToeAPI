using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public abstract class CrudRepositoryBase<TEntity>: ICrudRepository<TicTacToeDbContext, TEntity> 
    where TEntity : class, IHasId
{ //TODO: добавить сохранение бд
    public TicTacToeDbContext DbContext { get; }

    protected CrudRepositoryBase(TicTacToeDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public abstract DbSet<TEntity> GetDbSet();

    public abstract IQueryable<TEntity> GetQueryable();

    public async Task<IEnumerable<TEntity>> ReadAllAsync() => await GetQueryable().ToArrayAsync();

    public async Task<TEntity?> CreateAsync(Func<Guid, TEntity?> entityProducer)
    {
        var entity = entityProducer.Invoke(Guid.NewGuid());
        if (entity == null)
            return null;

        var entry = await GetDbSet().AddAsync(entity);
        return entry.Entity;
    }

    public async Task<TEntity?> ReadAsync(Guid id)
    {
        return await GetQueryable().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TEntity?> UpdateAsync(TEntity source)
    {
        var entry = GetDbSet().Update(source);
        return entry.Entity;
    }

    public async Task<TEntity?> DeleteAsync(Guid id)
    {
         var entity = await ReadAsync(id);
         return entity ?? null;
    }
}