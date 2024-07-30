using ChatApp.Components;
using ChatApp.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

using ChatApp.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using ChatApp;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSignalR();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions => {
    cookieOptions.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    cookieOptions.LoginPath = "/Forbidden";
    cookieOptions.AccessDeniedPath = "/Forbidden";


    cookieOptions.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.Redirect(cookieOptions.LoginPath);
        return Task.FromResult(0);
    };
});

builder.Services.AddAuthorization(options =>
{
});

/*var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);*/

builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<ChatsRepository>();
builder.Services.AddScoped<MessagesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ChatApp.Client._Imports).Assembly);


app.MapHub<ChatHub>("/chathub");


var userGroup = app.MapGroup("/api");
userGroup.MapEndpoints();

/*app.MapPost("api/Logout", async (ClaimsPrincipal user, HttpContext httpContext, [FromForm] string returnUrl) =>
{
    await httpContext.SignOutAsync();
    httpContext.Response.Redirect(returnUrl);
});

app.MapPost("api/chat/{chatName}/messages", async (ClaimsPrincipal user, HttpContext httpContext, [FromRoute] string chatName) =>
{
    
});*/

app.Run();
