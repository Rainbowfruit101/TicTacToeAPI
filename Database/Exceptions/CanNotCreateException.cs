namespace Database.Exceptions;

public class CanNotCreateException : Exception
{
    public CanNotCreateException(Type entityType) : base($"Can not create{entityType.FullName}")
    {
        
    }
}