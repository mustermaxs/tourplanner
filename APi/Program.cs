using Tourplanner.Services;

namespace Tourplanner;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Tourplanner.Repositories;
using Tourplanner.Infrastructure;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<TourContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Register other services
        RegisterServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        app.MapControllers();

        // Ensure database is created
        CreateDbIfNotExists(app);

        app.Run();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<DbContext, TourContext>();
        services.AddTransient<IServiceProvider, ServiceProvider>();
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IRatingService, RatingService>();
        services.AddTransient<IChildFriendlinessService, ChildFriendlinessService>();
        services.AddScoped<ITourLogRepository, TourLogRepository>();
        services.AddScoped<ITourRepository, TourRepository>();

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

    private static void CreateDbIfNotExists(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<TourContext>();
                context.Database.EnsureCreated(); // or your custom DbInitializer
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the database.");
            }
        }
    }
}