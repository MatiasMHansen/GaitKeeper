using GaitKeeper.WebAssembly;
using GaitKeeper.WebAssembly.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Konfigurer HttpClient til at pege på din Gateway
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7019/") }); // Gateway Uri 

builder.Services.AddScoped<SaveGaitDataService>();

await builder.Build().RunAsync();
