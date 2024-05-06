namespace Tourplanner.Entities.Tour;

using Tourplanner.Infrastructure;

public record GetTourCommand : ICommand
{
    public int TourId {get; set;}
}

public class GetTourCommandHandler : ICommandHandler
{
    private ApplicationDbContext dbContext;
    public GetTourCommandHandler(ApplicationDbContext ctx)
    {
        dbContext = ctx;
    }
    public override async Task<object> Handle(ICommand command)  // TODO implement
    {
        throw new NotImplementedException();
    }
}