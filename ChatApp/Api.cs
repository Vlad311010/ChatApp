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
            group.MapPost("/chat/{chatName}/leave", LeaveChat).WithName("LeaveChat");
            group.MapPost("/chat/{chatName}/invite/{userName}", InviteToChat).WithName("InviteUser");
            // group.MapGet("/chat/checkName/{chatName}", IsChatNameClaimed).WithName("IsChatNameClaimed");

            group.MapGet("/chats/my", MyChats).WithName("MyChats");
            group.MapGet("/chats/public", PublicChats).WithName("PublicChats");

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
                chatGroup.Memebers.Contains(new ChatGroupMembers(chatGroup, user));
            return Results.Ok(new BooleanResponce(isChatParticipant));
        }

        private static async Task<IResult> JoinChat(HttpContext httpContext, IUsersRepository usersRepo, IChatsRepository chatGroupsRepo,
            [FromRoute] string chatName)
        {
            if (!httpContext.User.Identity!.IsAuthenticated)
                return Results.Forbid();

            User? user = await usersRepo.GetByLogin(httpContext.User.Identity.Name!);         
            await chatGroupsRepo.AddUser(user!.Id, chatName);

            return Results.NoContent();
        }

        private static async Task<IResult> LeaveChat(HttpContext httpContext, IUsersRepository usersRepo, IChatsRepository chatGroupsRepo,
            [FromRoute] string chatName)
        {
            if (!httpContext.User.Identity!.IsAuthenticated)
                return Results.Forbid();

            User? user = await usersRepo.GetByLogin(httpContext.User.Identity.Name!);
            await chatGroupsRepo.RemoveUser(user!.Id, chatName);

            return Results.NoContent();
        }

        private static async Task<IResult> InviteToChat(HttpContext httpContext, IUsersRepository usersRepo, IChatsRepository chatGroupsRepo,
            [FromRoute] string chatName, [FromRoute] string userName)
        {
            User? userToInvite = await usersRepo.GetByLogin(userName);
            if (userToInvite == null)
                return Results.NotFound($"User {userName} not found");

            await chatGroupsRepo.AddUser(userToInvite.Id, chatName);
            return Results.NoContent();
        }

        /*public static async Task<IResult> IsChatNameClaimed(HttpContext httpContext, IChatsRepository chatGroupsRepo, [FromRoute] string chatName)
        {
            return await chatGroupsRepo.GetByName(chatName) == null
                    ? Results.Ok(new BooleanResponce(false))
                    : Results.Ok(new BooleanResponce(true));
        }*/

        public static async Task<IResult> MyChats(HttpContext httpContext, IChatsRepository chatGroupsRepo)
        {
            if (!httpContext.User.Identity!.IsAuthenticated)
                return Results.Forbid();

            string userName = httpContext.User.Identity.Name!;
            ChatGroup[] chats = (await chatGroupsRepo.UserChats(userName)).ToArray();

            return Results.Ok(chats);
        }

        public static async Task<IResult> PublicChats(HttpContext httpContext, IChatsRepository chatGroupsRepo)
        {
            ChatGroup[] chats = (await chatGroupsRepo.All(false)).ToArray();
            return Results.Ok(chats);
        }

    }
}
