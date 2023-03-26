namespace WebApi.Exceptions;

public class ServerStartException : Exception
{
    public ServerStartException(string message) : base(message)
    {
    }
}