using Database;
using Models;
using Models.Enums;
using Models.Exceptions;

namespace Services;

public class GameService
{
    private readonly ICrudRepository<TicTacToeDbContext, SessionModel> _repository;

    public GameService(ICrudRepository<TicTacToeDbContext, SessionModel> repository)
    {
        _repository = repository;
    }

    public SessionModel SetCell(SessionModel session, PlayerModel player, int x, int y)
    {
        if (!PlayerCanMove(session, player))
            throw new Exception(); //MoveNotAllowedException()

        if (!PositionIsCorrect(x, y))
            throw new InvalidPositionException(x, y);

        var cell = session[x, y];
        if(cell.State != CellState.Empty)
            throw new Exception(); //MoveNotAllowedException()

        cell.State = session.State switch
        {
            SessionState.MovePlayerO => CellState.O,
            SessionState.MovePlayerX => CellState.X,
            SessionState.Pending or SessionState.Completed or _ => throw new Exception() //MoveNotAllowedException()
        };

        return session;
    }

    private bool PlayerCanMove(SessionModel session, PlayerModel candidatePlayer)
    {
        Guid? movePlayerId = session.State switch
        {
            SessionState.MovePlayerO => session.PlayerO!.Id,
            SessionState.MovePlayerX => session.PlayerX!.Id,
            SessionState.Pending or SessionState.Completed or _ => null
        };

        return movePlayerId != null && candidatePlayer.Id == movePlayerId;
    }
    
    private bool PositionIsCorrect(int x, int y) => (x >= 0 && x < 3) && (y >= 0 && y < 3);
}