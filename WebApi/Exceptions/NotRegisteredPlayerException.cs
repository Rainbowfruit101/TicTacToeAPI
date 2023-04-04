namespace WebApi.Exceptions;

public class NotRegisteredPlayerException : Exception
{
    public NotRegisteredPlayerException(string email) : base($"Player with email '{email}' not registered")
    {
        
    }
}