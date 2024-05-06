namespace Tourplanner;

using Tourplanner.Infrastructure;

public abstract class IMediator
{
    protected Dictionary<string, ICommandHandler> _CommandCommandHandlerMapping = new Dictionary<string, ICommandHandler>();

    public virtual object Send(ICommand command)
    {
        var commandName = nameof(command);
        if (!_CommandCommandHandlerMapping.TryGetValue(commandName, out ICommandHandler? commandHandler))
        {
            throw new Exception($"Command {commandName} unkown");
        }

        return commandHandler.Handle(command);
    }
    public virtual bool Register<TCommand, TCommandHandler>()
        where TCommand : ICommand
        where TCommandHandler: ICommandHandler, new()
    {
        var commandName = typeof(TCommand).Name;
        return _CommandCommandHandlerMapping.TryAdd(commandName, new TCommandHandler());
    }
}



public class Mediator : IMediator
{
}