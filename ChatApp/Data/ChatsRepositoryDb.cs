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

        public async Task<ChatGroup?> GetByName(string chatName)
        {
            return await dbContext.ChatGroups.Where(u => u.Name == chatName).SingleOrDefaultAsync();
        }
    }
}
