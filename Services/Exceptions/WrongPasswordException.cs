namespace Services.Exceptions;

public class WrongPasswordException : Exception
{
    public WrongPasswordException(string password) : base($"Wrong password {password}")
    {
    }
}