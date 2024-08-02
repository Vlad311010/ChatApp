using ChatApp.Client.Models;
using ChatApp.Interfaces;

namespace ChatApp.Data
{
    public class MessagesRepositoryRuntime : IMessagesRepository
    {
        private readonly IChatsRepository chatsRepository;
        public MessagesRepositoryRuntime(IChatsRepository chatsRepository) 
        {
            this.chatsRepository = chatsRepository;
        }

        private List<Message> messages = new List<Message>()
        {
            new Message() { ChatId = 0, UserId = 0, CreatedAt = DateTime.UtcNow, Content = "Hello, world" },
        };

        public Task<IEnumerable<Message>> ChatMessages(int chatId)
        {
            return Task.FromResult(messages.Where(u => u.ChatId == chatId));
        }

        public Task<IEnumerable<Message>> ChatMessages(string chatName)
        {
            return Task.FromResult(messages.Where(u => u.ChatId == 0));
            // return messages.Where(u => u.Chat.Name == chatName);
        }

        public Task SendMessage(int userId, int chatId, string content)
        { 
            Message message = new Message(userId, chatId, content);
            messages.Add(message);
            return Task.CompletedTask;
        }

        public async Task SendMessage(int userId, string chatName, string content)
        {
            ChatGroup? chat = await chatsRepository.GetByName(chatName);
            if (chat == null)
                return;

            int chatId = chat.Id;
            Message message = new Message(userId, chatId, content);
            messages.Add(message);
        }
    }
}
