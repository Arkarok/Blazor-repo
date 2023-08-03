using BlazorPeliculas.Client;
using BlazorPeliculas.Client.Repositorios;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<ServicioSingleton>();
builder.Services.AddTransient<ServicioTransient>();
builder.Services.AddSingleton<Irepositorio, Repositorio>();

await builder.Build().RunAsync();
