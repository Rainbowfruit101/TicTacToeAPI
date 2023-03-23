using Models.Enums;
using Models.Exceptions;

namespace Models;

public class SessionModel : IHasId
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public PlayerModel? PlayerX { get; set; }
    public PlayerModel? PlayerO { get; set; }
    public SessionState State { get; set; }
    public CellModel[] Cells { get; set; }

    public CellModel this[int x, int y]
    {
        get
        {
            if (x is < 0 or > 2 ||
                y is < 0 or > 2)
            {
                throw new InvalidPositionException(x, y);
            }

            return Cells.First(c => c.X == x && c.Y == y);
        }
    }
}