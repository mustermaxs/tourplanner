namespace Client.Exceptions;

public class DaoException : Exception
{
    public DaoException(){}

    public DaoException(string message) : base(message)
    {
    }
    public DaoException(string message, Exception inner) : base(message, inner)
    {
    }
}