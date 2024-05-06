namespace Tourplanner.Infrastructure;

using Tourplanner.DTOs;

public abstract class ICommandHandler
{
    public virtual async Task<TDto> Handle<TDto>(ICommand command) where TDto : Dto
    {
        throw new NotImplementedException();
    }
}