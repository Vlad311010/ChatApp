using ChatApp.Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using ChatApp.Migrations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.Data;
using ChatApp.Client.ApiUtils;
using System.Linq;

namespace ChatApp
{

    // TODO: Aunthentication, authorization
    public static class Api
    {
        public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/logout", Logout).WithName("Logout");
            group.MapPost("/login", Login).WithName("Login");
            group.MapPost("/AutRefresh", AuthenticationRefresh).WithName("AuthenticationRefresh");

            group.MapGet("/chat/{chatName}/messages", GetChatMessages).WithName("GetChatMessages");
            group.MapGet("/chat/{chatName}/isParticipant/{userName}", IsChatParticipant).WithName("IsChatParticipant");
            group.MapPost("/chat/{chatName}/join", JoinChat).WithName("JoinChat");

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

        private static async Task<IResult> AuthenticationRefresh(HttpContext httpContext, IUsersRepository users)
        {
            string? loggedInUserLogin = httpContext.User.Identity?.Name;
            if (loggedInUserLogin == null)
                return Results.NoContent();

            User? user = await users.GetByLogin(loggedInUserLogin);
            if (user == null)
            {
                return Results.NoContent();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Login),
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

        private static async Task<IResult> GetChatMessages(HttpContext httpContext, IMessagesRepository messagesRepo, [FromRoute] string chatName)
        {
            Message[] messages = (await messagesRepo.ChatMessages(chatName)).ToArray();
            return Results.Ok(messages);
        }

        private static async Task<IResult> IsChatParticipant(HttpContext httpContext, IUsersRepository usersRepo, IChatsRepository chatGroupsRepo,
            [FromRoute] string chatName, [FromRoute] string userName)
        {
            // TODO: Make endpoint accessable only from owner/admin.

            User? user = await usersRepo.GetByLogin(userName);
            ChatGroup? chatGroup = await chatGroupsRepo.GetByName(chatName, true);


            bool isChatParticipant = user != null && chatGroup != null && chatGroup.Memebers != null &&
                chatGroup.Memebers.Any(cg => cg.Equals(new ChatGroupMembers(chatGroup, user)));
            return Results.Ok(new BooleanResponce(isChatParticipant));
        }

        private static async Task<IResult> JoinChat(HttpContext httpContext, 
            IUsersRepository usersRepo, IChatsRepository chatGroupsRepo,

            [FromRoute] string chatName)
        {
            if (!httpContext.User.Identity!.IsAuthenticated)
                return Results.Forbid();
            

            User? user = await usersRepo.GetByLogin(httpContext.User.Identity.Name!);
            // ChatGroup? chatGroup = await chatGroupsRepo.GetByName(chatName);

            await chatGroupsRepo.AddUser(user!.Id, chatName);

            return Results.NoContent();
        }

    }
}
