namespace Client.Exceptions;

public class UserActionException : Exception, UserRelevantException
{
    public UserActionException(){}

    public UserActionException(string message) : base(message)
    {
    }
    public UserActionException(string message, Exception inner) : base(message, inner)
    {
    }
}