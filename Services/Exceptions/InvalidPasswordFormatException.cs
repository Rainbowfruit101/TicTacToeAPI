namespace Services.Exceptions;

public class InvalidPasswordFormatException : Exception
{
    public InvalidPasswordFormatException() : base("Password ....") //TODO: fix message
    {
        
    }
}