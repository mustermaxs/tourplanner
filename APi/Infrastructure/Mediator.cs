using System.Reflection;
using Tourplanner.Entities.Tour;
using Microsoft.EntityFrameworkCore;
using Tourplanner.DTOs;

namespace Tourplanner;

using Tourplanner.Infrastructure;

public abstract class IMediator
{
    protected Dictionary<string, Type> _CommandCommandHandlerMapping = new Dictionary<string, Type>();
    protected DbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;


    protected IMediator(DbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;

        RegisterPublishers();
    }

    public async Task<object?> Send(IRequest request)
    {
        var commandName = request.GetType().Name;
        if (!_CommandCommandHandlerMapping.TryGetValue(commandName, out Type? commandHandlerType))
        {
            throw new Exception($"Command {commandName} unknown");
        }

        var requestType =
            commandHandlerType.GetMethod("Handle")!
                .GetParameters()[0]
                .ParameterType;
        var responseType = commandHandlerType.GetMethod("Handle")!
            .ReturnType;

        var commandHandler = _serviceProvider.GetServices(typeof(ICommandHandler))
            .First(h => h?.GetType().Name == commandHandlerType.Name);
            
        var handleMethod = commandHandlerType.GetMethod("Handle");
        var commandResultTask = (Task)handleMethod!.Invoke(commandHandler, new object[] { request });

        await commandResultTask.ConfigureAwait(false);

        var commandResult = commandResultTask
            .GetType()
            .GetProperty("Result")
            ?.GetValue(commandResultTask);

        return commandResult;
    }

    public bool Register<TCommand, TCommandHandler>()
        where TCommand : IRequest
    {
        var commandName = typeof(TCommand).Name;
        return _CommandCommandHandlerMapping.TryAdd(commandName, typeof(TCommandHandler));
    }

    public abstract void RegisterPublishers();
}

public class Mediator : IMediator
{
    public Mediator(DbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }

    public override void RegisterPublishers()
    {
        Register<GetToursRequest, GetToursCommandHandler>();
        Register<GetTourByIdRequest, GetTourByIdCommandHandler>();
    }

    public void DiscoverPublishers()
    {
    }
}