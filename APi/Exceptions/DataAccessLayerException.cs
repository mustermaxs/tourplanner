namespace Tourplanner.Exceptions;

public class DataAccessLayerException : Exception
{
    public DataAccessLayerException(){}

    public DataAccessLayerException(string message) : base(message)
    {
    }
    public DataAccessLayerException(string message, Exception inner) : base(message, inner)
    {
    }
}