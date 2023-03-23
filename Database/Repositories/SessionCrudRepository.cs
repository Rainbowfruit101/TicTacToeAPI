using Microsoft.EntityFrameworkCore;
using Models;

namespace Database.Repositories;

public class SessionCrudRepository : CrudRepositoryBase<SessionModel>
{
    public SessionCrudRepository(TicTacToeDbContext dbContext) : base(dbContext)
    {
    }

    public override DbSet<SessionModel> GetDbSet() => DbContext.Sessions;

    public override IQueryable<SessionModel> GetQueryable()
    {
        return GetDbSet()
            .Include(s => s.PlayerO)
            .Include(s => s.PlayerX)
            .Include(s => s.Cells);
    }
}