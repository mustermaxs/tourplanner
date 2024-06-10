namespace Client.Exceptions;

public class ServiceLayerException : Exception, UserRelevantException
{
    public ServiceLayerException(){}

    public ServiceLayerException(string message) : base(message)
    {
    }
    public ServiceLayerException(string message, Exception inner) : base(message, inner)
    {
    }
}