namespace Tourplanner;

using Tourplanner.Infrastructure;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

#nullable disable warnings

internal class Program
{
    private static WebApplicationBuilder? builder;
    private static WebApplication? app;
    public static DbContext? appContext;


    static void Main(string[] args)
    {

        var host = CreateHostBuilder(args).Build();
        CreateDbIfNotExists(host);
        builder = WebApplication.CreateBuilder(args);

        host.Run();
    }



    private static void CreateDbIfNotExists(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<TourContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }

    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}