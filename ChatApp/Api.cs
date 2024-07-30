using ChatApp.Data;
using ChatApp.Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp
{
    public static class Api
    {
        public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/logout", Logout).WithName("Logout");
            group.MapGet("/chat/{chatName}/messages", GetChatMessages).WithName("GetChatMessages");

            return group;
        }

        private static async Task Logout(ClaimsPrincipal user, HttpContext httpContext, [FromForm] string returnUrl)
        {
            await httpContext.SignOutAsync();
            httpContext.Response.Redirect(returnUrl);
        }

        private static async Task<IEnumerable<Message>> GetChatMessages(HttpContext httpContext, MessagesRepository messagesRepo, [FromRoute] string chatName)
        {
            return await Task.FromResult(messagesRepo.ChatMessages(chatName)) ;
        }
    }
}
