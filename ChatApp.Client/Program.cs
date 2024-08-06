using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatApp.Client;
using ChatApp.Client.Services;
using ChatApp.Client.Models;
using System;
using ChatApp.Client.ApiUtils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// builder.Services.AddAuthorizationCore();
// builder.Services.AddCascadingAuthenticationState();
// builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<ApiAccess>();
builder.Services.AddSingleton<ClientSideEvents>();


WebAssemblyHost host = builder.Build();

/*var logger = host.Services.GetRequiredService<ILoggerFactory>()
    .CreateLogger<Program>();*/

// restore active authentication state;
using (var scope = host.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    ApiAccess apiAccessor = service.GetService<ApiAccess>()!;
    CustomAuthenticationStateProvider authenticationStateProvider = service.GetService<CustomAuthenticationStateProvider>()!;

    UserData? userData = await apiAccessor.AuthenticationRefreshAsync();
    if (userData == null)
        authenticationStateProvider.SignOut();
    else
        authenticationStateProvider.LoginUser(userData);
}

await host.RunAsync();