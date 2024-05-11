using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tourplanner.DTOs;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;
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
        services.AddScoped<ITourLogRepository, TourLogRepository>();
        services.AddScoped<ITourRepository, TourRepository>();
        
        AddCommandHandlers(services);
        
        // services.AddTransient<ICommandHandler, GetToursCommandHandler>();
    }

    protected void AddCommandHandlers(IServiceCollection services)
    {
        services.AddScoped<ICommandHandler, GetToursCommandHandler>();
        services.AddScoped<ICommandHandler, GetTourByIdCommandHandler>();
        services.AddScoped<ICommandHandler, CreateTourCommandHandler>();
        services.AddScoped<ICommandHandler, UpdateTourCommandHandler>();
        services.AddScoped<ICommandHandler, DeleteTourCommandHandler>();
        services.AddScoped<ICommandHandler, GetTourLogsRequestHandler>();
        services.AddScoped<ICommandHandler, GetSingleTourLogRequestHandler>();
        services.AddScoped<ICommandHandler, CreateTourLogCommandHandler>();
        services.AddScoped<ICommandHandler, UpdateTourLogCommandHandler>();
        services.AddScoped<ICommandHandler, DeleteTourLogCommandHandler>();
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