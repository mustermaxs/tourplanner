using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Components;
using Client.Dao;
using Client.Services;
using Client.Utils;
using Client.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHttpService, HttpService>();
// builder.Services.AddScoped<Debouncer>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddTransient<IPopupService, PopupService>();
builder.Services.AddTransient<ITourDao, TourDao>();
builder.Services.AddTransient<ITourLogDao, TourLogDao>();
builder.Services.AddTransient<IReportService, ReportService>();

builder.Services.AddScoped<ToursPageViewModel>();
builder.Services.AddScoped<TourDetailsPageViewModel>();
builder.Services.AddScoped<LogsTableViewModel>();
builder.Services.AddScoped<TourAddPageViewModel>();
builder.Services.AddScoped<TourEditPageViewModel>();
builder.Services.AddScoped<TourLogPageViewModel>();
builder.Services.AddScoped<TourLogAddPageViewModel>();
builder.Services.AddScoped<PopupViewModel>();
builder.Services.AddScoped<SearchPageViewModel>();

await builder.Build().RunAsync();