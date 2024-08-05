using ChatApp.Client.Models;
using ChatApp.Client.Pages;
using ChatApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class ChatsRepositoryRuntime : IChatsRepository
    {
        private List<ChatGroup> chatGroups = new List<ChatGroup>()
        {
            new ChatGroup() { Id = 0, Name = "Test00", OwnerId = 0 },
            new ChatGroup() { Id = 1, Name = "Test01", OwnerId = 1 },
            new ChatGroup() { Id = 2, Name = "Test02", OwnerId = 0 }
        };

        public Task<IEnumerable<ChatGroup>> All()
        {
            return Task.FromResult<IEnumerable<ChatGroup>>(chatGroups);
        }

        public Task<ChatGroup?> GetByName(string chatName, bool includeMembers = false)
        {
            return Task.FromResult(chatGroups.Where(u => u.Name == chatName).SingleOrDefault());
        }

        public Task AddUser(int userId, int chatId)
        {
            /*ChatGroup chatGroup = chatGroups.Where(c => c.Id == chatId).Single();
            User user = new User(userId.ToString(), "");
            user.Id = userId;
            chatGroup.Participants.Add(user);
            return Task.CompletedTask;*/
            throw new NotImplementedException();
        }

        public Task AddUser(int userId, string chatName)
        {
            throw new NotImplementedException();
        }

    }
}
