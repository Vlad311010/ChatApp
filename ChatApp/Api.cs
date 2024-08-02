using ChatApp.Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using ChatApp.Client;
using ChatApp.Migrations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;

namespace ChatApp
{
    public static class Api
    {
        public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/logout", Logout).WithName("Logout");
            group.MapPost("/login", Login).WithName("Login");
            group.MapGet("/chat/{chatName}/messages", GetChatMessages).WithName("GetChatMessages");

            return group;
        }

        private static async Task<IResult> Login(HttpContext httpContext, IUsersRepository users, [FromBody] UserData loginRequest)
        {
            if (loginRequest == null)
                return Results.BadRequest();
            else if (string.IsNullOrEmpty(loginRequest.Login) /*|| string.IsNullOrEmpty(loginRequest.Password)*/)
                return Results.Problem(statusCode: 422, title: "Invalid request parameters");

            User? user = await users.GetByLogin(loginRequest.Login);
            if (user == null)
            {
                return Results.NotFound();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginRequest.Login),
                new Claim(ClaimTypes.NameIdentifier, loginRequest.Login),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            user.Password = "";
            return Results.Ok(user);
            
        }

        private static async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
            httpContext.Response.Redirect("\\");
        }

        private static async Task<IResult> GetChatMessages(HttpContext httpContext, IMessagesRepository messagesRepo, [FromRoute] string chatName)
        {
            Message[] messages = (await messagesRepo.ChatMessages(chatName)).ToArray();
            return Results.Ok(messages);
        }

    }
}
