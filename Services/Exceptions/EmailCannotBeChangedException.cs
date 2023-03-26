namespace Services.Exceptions;

public class EmailCannotBeChangedException : Exception
{ 
    public EmailCannotBeChangedException(string email) : base($"Email {email} cannot be changed")
    {
        
    }
}