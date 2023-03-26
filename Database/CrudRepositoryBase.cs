using Database.Exceptions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public abstract class CrudRepositoryBase<TEntity>: ICrudRepository<TicTacToeDbContext, TEntity> 
    where TEntity : class, IHasId
{
    public TicTacToeDbContext DbContext { get; }

    protected CrudRepositoryBase(TicTacToeDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public abstract DbSet<TEntity> GetDbSet();

    public abstract IQueryable<TEntity> GetQueryable();

    public async Task<IEnumerable<TEntity>> ReadAllAsync() => await GetQueryable().ToArrayAsync();

    public async Task<TEntity> CreateAsync(Func<Guid, TEntity> entityProducer)
    {
        var entity = entityProducer.Invoke(Guid.NewGuid());
        if (entity == null)
            throw new CanNotCreateException(typeof(TEntity));

        var entry = await GetDbSet().AddAsync(entity);

        await DbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<TEntity> ReadAsync(Guid id)
    {
        var result = await GetQueryable().FirstOrDefaultAsync(e => e.Id == id);
        if (result == null)
        {
            throw new NotFoundException(typeof(TEntity));
        }
        return result;
    }

    public async Task<TEntity> UpdateAsync(TEntity source)
    {
        var entry = GetDbSet().Update(source);
        await DbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<TEntity> DeleteAsync(Guid id)
    {
         var entity = await ReadAsync(id);
         await DbContext.SaveChangesAsync();
         return entity;
    }
}