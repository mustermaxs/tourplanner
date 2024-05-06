namespace Tourplanner.Entities.Tour;

using Tourplanner.Infrastructure;

public record DeleteTourCommand : ICommand
{
    public int TourId {get; set;}
    public int UserId {get; set;}
}

public class DeleteTourCommandHandler : ICommandHandler
{
    public override async Task<object> Handle(ICommand command)
    {
        throw new NotImplementedException();
    }
}