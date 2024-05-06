namespace Tourplanner;

using Tourplanner.Infrastructure;
using Tourplanner.Entities.Tour;
using Microsoft.EntityFrameworkCore;

#nullable disable warnings

internal class Program
{
    private static WebApplicationBuilder? builder;
    private static WebApplication? app;

    
    static void Main(string[] args)
    {
        builder = WebApplication.CreateBuilder(args);
        app = builder.Build();

        RegisterServices(builder.Services);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void RegisterServices(IServiceCollection services)
    {

        builder.Services.AddDbContext<TourDbContext>(opts => {
            // postgres
            opts.UseNpgsql(
            builder.Configuration["ConnectionStrings:PostgresConnection"]);
        });

        

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<ICommandHandler, GetTourCommandHandler>();
        services.AddSingleton<IMediator, Mediator>();
    }
}