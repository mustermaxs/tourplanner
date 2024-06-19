using System.Reflection;
using Api.Services.Logging;
using Tourplanner.Exceptions;
using LoggerFactory = Api.Services.Logging.LoggerFactory;


namespace Tourplanner;

using Tourplanner.Infrastructure;

public abstract class IMediator
{
    protected static Dictionary<string, Type> _CommandCommandHandlerMapping = new Dictionary<string, Type>();
    private readonly IServiceProvider _serviceProvider;
    protected static ILoggerWrapper Logger = LoggerFactory.GetLogger();


    protected IMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<dynamic?> Send(IRequest request)
    {
        var commandHandlerType = GetCommandHandlerType(request);
        var commandHandler = GetCommandHandler(commandHandlerType);
        var handleMethod = GetHandleMethod(commandHandlerType);
    
        var commandResult = await ExecuteHandleMethod(commandHandler, handleMethod, request);
        return commandResult;
    }

    public Type GetCommandHandlerType(IRequest request)
    {
        var commandName = request.GetType().Name;
        if (!_CommandCommandHandlerMapping.TryGetValue(commandName, out Type? commandHandlerType))
        {
            Logger.Fatal($"[Mediator] Command {commandName} unknown");
            throw new InfrastructureException($"[Mediator] Command {commandName} unknown");
        }
        return commandHandlerType;
    }

    public object GetCommandHandler(Type commandHandlerType)
    {
        var commandHandler = _serviceProvider.GetServices(typeof(ICommandHandler))
            .First(h => h?.GetType().Name == commandHandlerType.Name);
        if (commandHandler == null)
        {
            Logger.Fatal($"[Mediator] Handler not found for command handler type {commandHandlerType.Name}");
            throw new InfrastructureException($"[Mediator] Handler not found for command handler type {commandHandlerType.Name}");
        }
        return commandHandler;
    }

    public MethodInfo GetHandleMethod(Type commandHandlerType)
    {
        var handleMethod = commandHandlerType.GetMethod("Handle");
        if (handleMethod == null)
        {
            throw new InfrastructureException($"[Mediator] Handle method not found for command handler type {commandHandlerType.Name}");
        }
        return handleMethod;
    }

    private async Task<dynamic?> ExecuteHandleMethod(object commandHandler, MethodInfo handleMethod, IRequest request)
    {
        var commandResultTask = (Task)handleMethod.Invoke(commandHandler, new object[] { request });
        await commandResultTask.ConfigureAwait(false);

        var commandResult = commandResultTask.GetType().GetProperty("Result")?.GetValue(commandResultTask);
        return commandResult;
    }

    public bool Register<TCommand, TCommandHandler>()
        where TCommand : IRequest
    {
        var commandName = typeof(TCommand).Name;
        return _CommandCommandHandlerMapping.TryAdd(commandName, typeof(TCommandHandler));
    }

    public static void DiscoverPublishers(Assembly assembly)
    {
        var assemblyTypes = assembly.GetTypes();
        var requests = assemblyTypes.Where(t => typeof(IRequest).IsAssignableFrom(t) && !t.IsInterface);
        var requestHandlers = assemblyTypes.Where(t => IsDerivableFromBaseClass(t) && !t.IsAbstract);

        foreach (var request in requests)
        {
            var responsibleHandler = requestHandlers.SingleOrDefault(r => r.Name == $"{request.Name}Handler");
            if (responsibleHandler is null)
            {
                Logger.Fatal($"[Mediator] Couldn't find handler for request type {request.Name}");
                throw new Exception($"Mediator couldn't find handler for request type {request.Name}");
            }

            _CommandCommandHandlerMapping.TryAdd(request.Name, responsibleHandler);
        }
    }

    private static bool IsDerivableFromBaseClass(Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var currentType = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (typeof(RequestHandler<,>) == currentType)
            {
                return true;
            }

            toCheck = toCheck.BaseType;
        }

        return false;
    }
}

public class Mediator : IMediator
{
    public Mediator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}