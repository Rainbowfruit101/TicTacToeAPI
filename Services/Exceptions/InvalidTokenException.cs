namespace Services.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException() : base("Invalid token")
    {
    }
}