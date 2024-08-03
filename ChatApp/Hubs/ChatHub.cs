using ChatApp.Client.Pages;
using ChatApp.Data;
using ChatApp.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Interfaces;
using System.Text.RegularExpressions;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessagesRepository messagesRepo;
        private readonly IChatsRepository ChatsRepo;
        public ChatHub(IMessagesRepository messagesRepo, IChatsRepository chatsRepo)
        {
            if (messagesRepo == null)
            {
                throw new NullReferenceException("messagesRepo");
            }
            if (chatsRepo == null)
            {
                throw new NullReferenceException("chatsRepo");
            }

            this.messagesRepo = messagesRepo;
            this.ChatsRepo = chatsRepo;
        }

        public async Task SendMessage(int userId, string userName, string chatName, string message)
        {            
            ChatGroup? chatGroup = await ChatsRepo.GetByName(chatName);
            if (chatGroup == null)
            {
                // throw exception ???
                return;
            }

            await messagesRepo.SendMessage(userId, chatGroup.Id, message);
            await Clients.Group(chatName).SendAsync("ReceiveMessage", userId, userName, chatGroup.Id, message);
        }

        [Authorize]
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GroupConnect", $"{Context.UserIdentifier} has joined the group {groupName}.");
            // Console.WriteLine($"{Context.UserIdentifier} has joined the group {groupName}.");
        }

        [Authorize]
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GroupDisconnect", $"{Context.UserIdentifier} has left the group {groupName}.");
            // Console.WriteLine($"{Context.UserIdentifier} has left the group {groupName}.");
        }

    }
}
