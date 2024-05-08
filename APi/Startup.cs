using Microsoft.EntityFrameworkCore;

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
  }
 
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }
            
    app.UseRouting();
  }
}