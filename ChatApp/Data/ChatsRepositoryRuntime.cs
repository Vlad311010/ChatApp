using ChatApp.Client.Models;
using ChatApp.Client.Pages;
using ChatApp.Interfaces;

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

        public Task<ChatGroup?> GetByName(string chatName)
        {
            return Task.FromResult(chatGroups.Where(u => u.Name == chatName).SingleOrDefault());
        }
    }
}
