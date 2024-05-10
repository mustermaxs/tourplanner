using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tourplanner.DTOs;
using Tourplanner.Entities.Tour;
using Tourplanner.Infrastructure;
using Tourplanner.Repositories;

namespace Tourplanner;

// https://www.tutorialsteacher.com/core/aspnet-core-startup

public class Startup
{
    public IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TourContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddControllers().AddJsonOptions(x =>{
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});

        services.AddTransient<DbContext, TourContext>();
        services.AddTransient<IServiceProvider, ServiceProvider>();
        services.AddTransient<IMediator, Mediator>();
        services.AddScoped<TourRepository>();
        AddCommandHandlers(services);
        // services.AddTransient<ICommandHandler, GetToursCommandHandler>();
    }

    protected void AddCommandHandlers(IServiceCollection services)
    {
        // var type = typeof(ICommandHandler);
        // var types = Assembly.GetExecutingAssembly()
        //     .GetTypes().Where(handlerType => handlerType is { IsClass: true, IsAbstract: false } && type.IsAssignableFrom(handlerType)).ToList();
        //
        // foreach (var handler in types)
        // {
        //     var genericArguments = handler.GetGenericArguments();
        //     var requestType =
        //         handler.GetMethod("Handle")!
        //             .GetParameters()[0]
        //             .ParameterType;
        //     var responseType = handler.GetMethod("Handle")!
        //         .ReturnType;
        //
        //     var requestHandlerType = typeof(RequestHandler<,>).MakeGenericType(requestType, responseType);
        //     services.AddTransient(typeof(ICommandHandler), requestHandlerType);
        //     // services.BuildServiceProvider();
        // }
        services.AddScoped<ICommandHandler, GetToursCommandHandler>();
        services.AddScoped<ICommandHandler, GetTourByIdCommandHandler>();
        services.AddScoped<ICommandHandler, CreateTourCommandHandler>();
        services.AddScoped<ICommandHandler, UpdateTourCommandHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}