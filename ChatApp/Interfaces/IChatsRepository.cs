using ChatApp.Client.ApiUtils;
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
        Task<BooleanResponce> CreateChat(ChatGroupView chatGroup, bool addCreatorToChat = false);
        Task<IEnumerable<ChatGroup>> UserChats(string userName);
    }
}
