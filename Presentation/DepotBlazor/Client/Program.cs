using Blazored.LocalStorage;
using DepotBlazor.Client;
using DepotBlazor.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddOptions();
builder.Services.AddBlazoredLocalStorage();

//MudBlazor
builder.Services.AddMudServices();
builder.Services.AddScoped<ITokenService, TokenService>();
await builder.Build().RunAsync();
