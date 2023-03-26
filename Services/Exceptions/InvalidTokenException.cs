namespace Services.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string token) : base($"Invalid token {token}")
    {
    }
}