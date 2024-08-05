using ChatApp.Client.Models;
using ChatApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class ChatsRepositoryDb : IChatsRepository
    {
        public readonly AppDbContext dbContext;
        public ChatsRepositoryDb(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ChatGroup>> All()
        {
            return await dbContext.ChatGroups.ToListAsync();
        }

        public async Task<ChatGroup?> GetByName(string chatName, bool includeMembers = false)
        {
            if (includeMembers)
                return await dbContext.ChatGroups.Where(c => c.Name == chatName).Include(c => c.Memebers).SingleOrDefaultAsync();
            else 
                return await dbContext.ChatGroups.Where(c => c.Name == chatName).SingleOrDefaultAsync();
        }

        public async Task AddUser(int userId, int chatId)
        {
            ChatGroup chatGroup = await dbContext.ChatGroups.Where(c => c.Id == chatId).Include(c => c.Memebers).SingleAsync();
            User user = await dbContext.Users.Where(u => u.Id == userId).SingleAsync();

            ChatGroupMembers chatGroupMembers = new ChatGroupMembers(chatGroup, user);
            if (!chatGroup.Memebers.Contains(chatGroupMembers))
            {
                chatGroup.Memebers.Add(chatGroupMembers);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddUser(int userId, string chatName)
        {
            ChatGroup chatGroup = await dbContext.ChatGroups.Where(c => c.Name == chatName).Include(c => c.Memebers).SingleAsync();
            User user = await dbContext.Users.Where(u => u.Id == userId).SingleAsync();

            /*Console.WriteLine((await dbContext.ChatGroups.Where(c => c.Name == "Test01").Include(c => c.Participants).SingleAsync()).Participants.Count());
            Console.WriteLine((await dbContext.ChatGroups.Where(c => c.Name == "Test02").Include(c => c.Participants).SingleAsync()).Participants.Count());
            Console.WriteLine((await dbContext.ChatGroups.Where(c => c.Name == "Test03").Include(c => c.Participants).SingleAsync()).Participants.Count());*/
            if (chatGroup.Memebers == null)
            {
                chatGroup.Memebers = new List<ChatGroupMembers>();
            }

            ChatGroupMembers chatGroupMembers = new ChatGroupMembers(chatGroup, user);
            if (!chatGroup.Memebers.Contains(chatGroupMembers))
            {
                chatGroup.Memebers.Add(chatGroupMembers);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
