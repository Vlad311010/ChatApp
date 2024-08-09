using ChatApp.Client.Models;

namespace ChatApp.Interfaces
{
    public interface IMessagesRepository
    {
        Task<IEnumerable<Message>> ChatMessages(string chatName, int messagesToTake);
        Task<IEnumerable<Message>> ChatMessages(string chatName, int startMessageId, int messagesToTake);
        Task SendMessage(int userId, int chatId, string content);
        Task SendMessage(int userId, string chatName, string content);
    }
}
