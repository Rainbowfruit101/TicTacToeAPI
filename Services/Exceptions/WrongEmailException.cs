namespace Services.Exceptions;

public class WrongEmailException : Exception
{
    public WrongEmailException(string email) : base($"Wrong email {email}")
    {
        
    }
}