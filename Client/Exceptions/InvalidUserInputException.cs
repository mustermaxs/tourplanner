namespace Client.Exceptions;

public class InvalidUserInputException : Exception, UserRelevantException
{
    public InvalidUserInputException(){}

    public InvalidUserInputException(string message) : base(message)
    {
    }
    public InvalidUserInputException(string message, Exception inner) : base(message, inner)
    {
    }
}