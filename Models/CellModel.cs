using Models.Enums;

namespace Models;

public class CellModel: IHasId
{
    public Guid Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public CellState State { get; set; }
}