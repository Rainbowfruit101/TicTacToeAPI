using Microsoft.EntityFrameworkCore;
using Models;

namespace Database.Repositories;

public class PlayersCrudRepository: CrudRepositoryBase<PlayerModel>
{
    public PlayersCrudRepository(TicTacToeDbContext dbContext) : base(dbContext)
    {
    }

    public override DbSet<PlayerModel> GetDbSet() => DbContext.Players;

    public override IQueryable<PlayerModel> GetQueryable()
    {
        return GetDbSet()
            .Include(p => p.Sessions)
            .ThenInclude(s => s.PlayerX)
            .Include(p => p.Sessions)
            .ThenInclude(s => s.PlayerO);
    }
}