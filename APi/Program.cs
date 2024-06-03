using Tourplanner.Entities;
using Tourplanner.Entities.TourLogs.Commands;
using Tourplanner.Entities.Tours.Commands;
using Tourplanner.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Tourplanner.Repositories;
using Tourplanner.Infrastructure;
using Tourplanner.Entities.Tours;
using Tourplanner.Entities.TourLogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tourplanner;



internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        });
        builder.Services.AddDbContext<TourContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHttpClient();
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        RegisterServices(builder.Services);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        app.MapControllers();

        CreateDbIfNotExists(app);

        app.Run();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<DbContext, TourContext>();
        services.AddScoped<FileSystemHandler>();
        services.AddTransient<IServiceProvider, ServiceProvider>();
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IRatingService, RatingService>();
        services.AddTransient<IChildFriendlinessService, ChildFriendlinessService>();
        services.AddTransient<IReportService, ReportService>();
        services.AddScoped<ITileRepository, TileRepository>();
        services.AddScoped<ITourLogRepository, TourLogRepository>();
        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<ITileService, TileService>();
        
        services.AddTransient<IOpenRouteService, OpenRouteService>();

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
        services.AddScoped<ICommandHandler, GetTourReportCommandHandler>();
        services.AddScoped<ICommandHandler, GetSummaryReportCommandHandler>();
        services.AddScoped<ICommandHandler, GetSearchResultsQueryHandler>();
        services.AddScoped<ICommandHandler, GetGeoAutoCompleteQueryHandler>();
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