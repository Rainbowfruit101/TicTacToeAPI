using Database;
using Models;
using Models.Enums;

namespace Services;

public class SessionService
{
    private readonly ICrudRepository<TicTacToeDbContext, SessionModel> _repository;

    public SessionService(ICrudRepository<TicTacToeDbContext, SessionModel> repository)
    {
        _repository = repository;
    }

    public async Task<SessionModel> GetByIdAsync(Guid id)
    {
        var result = await _repository.ReadAsync(id);
        return result;
    }

    public async Task<SessionModel> FindJoinSession()
    {
        var result = _repository.GetQueryable()
            .Where(SearchCondition)
            .MinBy(s => s.CreationTime);
        if (result == null)
        {
            result = await _repository.CreateAsync(CreateSession);
        }

        return result;
    }

    private bool SearchCondition(SessionModel session)
    {
        if (session.State != SessionState.Pending)
        {
            return false;
        }

        if (session.PlayerO != null && session.PlayerX != null)
        {
            return false;
        }

        return true;
    }

    private SessionModel CreateSession(Guid id)
    {
        return new SessionModel()
        {
            Id = id,
            CreationTime = DateTime.Now,
            PlayerO = null,
            PlayerX = null,
            State = SessionState.Pending,
            Cells = CreateCells()
        };
    }

    public async Task<SessionModel> Update(SessionModel source)
    {
        var session = _repository.GetQueryable().FirstOrDefault(s => s.Id == source.Id);
        
        if (session==null)
        {
            throw new Exception();
        }

        return await _repository.UpdateAsync(source);
    }

    private CellModel[] CreateCells()
    {
        const int width = 3;
        const int height = 3;

        var x = 0;
        var y = 0;

        var cells = new List<CellModel>();
        for (var i = 0; i < width * height; i++)
        {
            cells.Add(new CellModel()
            {
                Id = Guid.NewGuid(),
                State = CellState.Empty,
                X = x,
                Y = y
            });

            x += 1;

            if (x >= width)
            {
                x = 0;
                y += 1;
            }
        }

        return cells.ToArray();
    }
}