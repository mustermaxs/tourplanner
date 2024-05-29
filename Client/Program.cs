using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Components;
using Client.Dao;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddTransient<IPopupService, PopupService>();
builder.Services.AddTransient<ITourDao, TourDao>();
builder.Services.AddTransient<ITourLogDao, TourLogDao>();
builder.Services.AddScoped<ToursPageViewModel>();
builder.Services.AddScoped<TourDetailsPageViewModel>();
builder.Services.AddScoped<TourAddPageViewModel>();
builder.Services.AddScoped<TourEditPageViewModel>();
builder.Services.AddScoped<TourLogPageViewModel>();
builder.Services.AddScoped<TourLogAddPageViewModel>();
builder.Services.AddScoped<PopupViewModel>();

await builder.Build().RunAsync();