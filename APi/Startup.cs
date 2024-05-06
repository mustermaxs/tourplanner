namespace Tourplanner;

// https://www.tutorialsteacher.com/core/aspnet-core-startup

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
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