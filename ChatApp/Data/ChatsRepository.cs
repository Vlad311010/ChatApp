using ChatApp.Client.Models;

namespace ChatApp.Data
{
    public class ChatsRepository
    {
        private List<ChatGroup> chatGroups = new List<ChatGroup>()
        {
            new ChatGroup() { Id = 0, Name = "Test00", OwnerId = 0 },
            new ChatGroup() { Id = 1, Name = "Test01", OwnerId = 1 },
            new ChatGroup() { Id = 2, Name = "Test02", OwnerId = 0 }
        };

        public IEnumerable<ChatGroup> All()
        {
            return chatGroups;
        }

        public ChatGroup? GetByName(string chatName)
        {
            return chatGroups.Where(u => u.Name == chatName).SingleOrDefault();
        }
    }
}
