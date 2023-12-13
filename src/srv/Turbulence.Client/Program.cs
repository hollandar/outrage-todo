using Turbulence.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Cadence.UI.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddHttpClient("Turbulence.ServerAPI")
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://localhost:7002"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ToasterService>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
