using System.Reflection;
using Api.Services.Logging;
using Tourplanner.Entities.Tours;
using Microsoft.EntityFrameworkCore;
using Tourplanner.DTOs;
using Tourplanner.Entities;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Entities.TourLogs.Commands;
using Tourplanner.Infrastructure;
using LoggerFactory = Api.Services.Logging.LoggerFactory;
using Tourplanner.Entities.Tours.Commands;
using Tourplanner.Exceptions;


namespace Tourplanner;

using Tourplanner.Infrastructure;

public abstract class IMediator
{
    protected static Dictionary<string, Type> _CommandCommandHandlerMapping = new Dictionary<string, Type>();
    protected DbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    protected ILoggerWrapper Logger = LoggerFactory.GetLogger();


    protected IMediator(DbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<object?> Send(IRequest request)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            Logger.Fatal($"Mediator failed: {e.Message}. Request: {request.ToString()} RequestType: {request.GetType().FullName}.");
            throw new Exception("Mediator failed");
        }
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
                throw new Exception($"Mediator couldn't find handler for request type {request.Name}");

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
    public Mediator(DbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}