namespace Tourplanner.Infrastructure;

using Tourplanner.DTOs;

public abstract class ICommandHandler
{
    public virtual async Task<Dto> Handle(ICommand command)
    {
        throw new NotImplementedException();
    }
}