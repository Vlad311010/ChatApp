using ChatApp.Client.Pages;
using ChatApp.Data;
using ChatApp.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessagesRepository messagesRepo;
        public ChatHub(MessagesRepository messagesRepo)
        {
            if (messagesRepo == null)
            {
                throw new NullReferenceException("messagesRepo");
            }
            this.messagesRepo = messagesRepo;
        }

        public async Task SendMessage(string userId, string chatName, string message)
        {            
            await Clients.All.SendAsync("ReceiveMessage", userId, chatName, message);
            messagesRepo.SendMessage(Int32.Parse(userId), chatName, message);
        }

        [Authorize]
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.UserIdentifier!, groupName);
            await Clients.Group(groupName).SendAsync("GroupConnect", $"{Context.UserIdentifier} has joined the group {groupName}.");
            Console.WriteLine($"{Context.UserIdentifier} has joined the group {groupName}.");
        }

        [Authorize]
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.UserIdentifier!, groupName);
            await Clients.Group(groupName).SendAsync("GroupDisconnect", $"{Context.UserIdentifier} has left the group {groupName}.");
            Console.WriteLine($"{Context.UserIdentifier} has left the group {groupName}.");
        }

    }
}
