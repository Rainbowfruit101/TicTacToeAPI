using Models.Enums;

namespace Models;

public class SessionModel : IHasId
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public PlayerModel? PlayerX { get; set; }
    public PlayerModel? PlayerO { get; set; }
    public SessionState State { get; set; }
    public CellModel[] Cells { get; set; }

    public CellModel this[int x, int y] => Cells.First(c => c.X == x && c.Y == y);
}