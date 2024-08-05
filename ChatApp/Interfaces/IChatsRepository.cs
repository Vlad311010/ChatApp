using ChatApp.Client.Models;

namespace ChatApp.Interfaces
{
    public interface IChatsRepository
    {
        Task<IEnumerable<ChatGroup>> All();
        Task<ChatGroup?> GetByName(string chatName, bool includeMembers = false);
        Task AddUser(int userId, int chatId);
        Task AddUser(int userId, string chatName);
    }
}
