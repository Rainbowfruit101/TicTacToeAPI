namespace Services.Exceptions;

public class NotRegisterPlayerException : Exception
{
    public NotRegisterPlayerException(string email) : base($"Not registered player with email {email}")
    {
        
    }
}