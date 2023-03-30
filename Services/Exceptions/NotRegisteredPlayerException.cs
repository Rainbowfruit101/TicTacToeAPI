namespace Services.Exceptions;

public class NotRegisteredPlayerException : Exception
{
    public NotRegisteredPlayerException(string email) : base($"Player with email '{email}' not registered")
    {
        
    }
}