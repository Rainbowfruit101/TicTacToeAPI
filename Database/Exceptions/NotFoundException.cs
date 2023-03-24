namespace Database.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(Type entityType) : base($"Not found entity{entityType.FullName}")
    {
    }
}