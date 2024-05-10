using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ToursViewModel>();
builder.Services.AddScoped<TourDetailsPageViewModel>();
builder.Services.AddScoped<TourAddPageViewModel>();
builder.Services.AddScoped<TourEditPageViewModel>();
builder.Services.AddScoped<TourLogPageViewModel>();


await builder.Build().RunAsync();