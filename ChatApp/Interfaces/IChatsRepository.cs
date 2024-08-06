using ChatApp.Client.Models;
using ChatApp.Client.ViewModels;

namespace ChatApp.Interfaces
{
    public interface IChatsRepository
    {
        Task<IEnumerable<ChatGroup>> All(bool includePrivate = true);
        Task<ChatGroup?> GetByName(string chatName, bool includeMembers = false);
        Task AddUser(int userId, string chatName);
        Task RemoveUser(int userId, string chatName);
        Task CreateChat(ChatGroupView chatGroup);
        Task<IEnumerable<ChatGroup>> UserChats(string userName);
    }
}
