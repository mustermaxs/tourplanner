namespace Tourplanner.Exceptions;

public class CreateTourException : Exception
{
    public CreateTourException(){}

    public CreateTourException(string message) : base(message)
    {
    }
    public CreateTourException(string message, Exception inner) : base(message, inner)
    {
    }
}