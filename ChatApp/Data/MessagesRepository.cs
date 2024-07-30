using ChatApp.Client.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;

namespace ChatApp.Data
{
    public class MessagesRepository
    {
        private readonly ChatsRepository chatsRepository;
        public MessagesRepository(ChatsRepository chatsRepository) 
        {
            this.chatsRepository = chatsRepository;
        }

        private List<Message> messages = new List<Message>()
        {
            new Message() { ChatId = 0, UserId = 0, CreatedAt = DateTime.UtcNow, Content = "Hello, world" },
        };

        public IEnumerable<Message> ChatMessages(int chatId)
        {
            return messages.Where(u => u.ChatId == chatId);
        }

        public IEnumerable<Message> ChatMessages(string chatName)
        {
            return messages.Where(u => u.Chat.Name == chatName);
        }

        public void SendMessage(int userId, int chatId, string content)
        { 
            Message message = new Message(userId, chatId, content);
            messages.Add(message);
        }

        public void SendMessage(int userId, string chatName, string content)
        {
            ChatGroup? chat = chatsRepository.GetByName(chatName);
            if (chat == null)
                return;

            int chatId = chat.Id;
            Message message = new Message(userId, chatId, content);
            messages.Add(message);
        }

    }
}
