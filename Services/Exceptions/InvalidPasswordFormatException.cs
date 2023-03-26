namespace Services.Exceptions;

public class InvalidPasswordFormatException : Exception
{
    public InvalidPasswordFormatException(string password) : base($"Invalid password format {password}")
    {
        
    }
}