namespace Models.Exceptions;

public class InvalidPositionException : Exception
{
    public int X { get; }
    public int Y { get; }

    public InvalidPositionException(int x, int y) : base($"Invalid position [{x},{y}].")
    {
        X = x;
        Y = y;
    }
}