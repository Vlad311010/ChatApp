using ChatApp.Client.Models;

namespace ChatApp.Interfaces
{
    public interface IChatsRepository
    {
        Task<IEnumerable<ChatGroup>> All();
        Task<ChatGroup?> GetByName(string chatName);
    }
}
