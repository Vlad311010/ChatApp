using ChatApp.Client.Models;
using ChatApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class MessagesRepositoryDb : IMessagesRepository
    {
        private readonly AppDbContext dbContext;
        private readonly IChatsRepository chatsRepository;
        public MessagesRepositoryDb(AppDbContext dbContext, IChatsRepository chatsRepository)
        {
            this.dbContext = dbContext;
            this.chatsRepository = chatsRepository;
        }

        public async Task<IEnumerable<Message>> ChatMessages(string chatName, int messagesToTake)
        {
            return await dbContext.Messages
                .Include(m => m.Chat)
                .Where(u => u.Chat.Name == chatName)
                .OrderByDescending(m => m.CreatedAt)
                .Take(messagesToTake)
                .Include(m => m.User)
                .Reverse()
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Message>> ChatMessages(string chatName, int startMessageId, int messagesToTake)
        {
            DateTime startMessageTime = (await dbContext.Messages.Where(m => m.Id == startMessageId).SingleAsync()).CreatedAt;

            return await dbContext.Messages
                .Include(m => m.Chat)
                .Where(m => m.Chat.Name == chatName &&  m.CreatedAt < startMessageTime)
                .OrderByDescending(m => m.CreatedAt)
                .Take(messagesToTake)
                .Include(m => m.User)
                .Reverse()
                .ToArrayAsync();
        }

        public async Task SendMessage(int userId, int chatId, string content)
        {
            Message message = new Message(userId, chatId, content);
            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();
        }

        public async Task SendMessage(int userId, string chatName, string content)
        {
            ChatGroup? chat = await chatsRepository.GetByName(chatName);
            if (chat == null)
                return;

            int chatId = chat.Id;
            await SendMessage(userId, chatId, content);
        }
    }
}
