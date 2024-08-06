using ChatApp.Client.Models;
using ChatApp.Client.ViewModels;
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

        public async Task<IEnumerable<ChatGroup>> All(bool includePrivate = true)
        {
            if (includePrivate)
                return await dbContext.ChatGroups.ToListAsync();
            else 
                return await dbContext.ChatGroups.Where(cg => cg.IsPublic).ToListAsync();
        }

        public async Task<ChatGroup?> GetByName(string chatName, bool includeMembers = false)
        {
            if (includeMembers)
                return await dbContext.ChatGroups.Where(c => c.Name == chatName).Include(c => c.Memebers).SingleOrDefaultAsync();
            else 
                return await dbContext.ChatGroups.Where(c => c.Name == chatName).SingleOrDefaultAsync();
        }

        public async Task AddUser(int userId, string chatName)
        {
            ChatGroup chatGroup = await dbContext.ChatGroups.Where(c => c.Name == chatName).Include(c => c.Memebers).SingleAsync();
            User user = await dbContext.Users.Where(u => u.Id == userId).SingleAsync();

            ChatGroupMembers chatGroupMembers = new ChatGroupMembers(chatGroup, user);
            if (!chatGroup.Memebers.Contains(chatGroupMembers))
            {
                chatGroup.Memebers.Add(chatGroupMembers);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveUser(int userId, string chatName)
        {
            ChatGroup chatGroup = await dbContext.ChatGroups.Where(c => c.Name == chatName).Include(c => c.Memebers).SingleAsync();
            User user = await dbContext.Users.Where(u => u.Id == userId).SingleAsync();

            ChatGroupMembers chatGroupMembers = new ChatGroupMembers(chatGroup, user);

            chatGroup.Memebers.Remove(chatGroupMembers);
            dbContext.SaveChanges();
        }

        public async Task<IEnumerable<ChatGroup>> UserChats(string userName)
        {
            User? user = await dbContext.Users.Where(u => u.Login == userName).SingleOrDefaultAsync();
            if (user == null)
                return new List<ChatGroup>();

            int[] userChats = dbContext.ChatGroupmembers.Where(m => m.UserId == user.Id).Select(m => m.ChatGroupId).ToArray();
            return dbContext.ChatGroups.Where(c => userChats.Contains(c.Id));
        }


        public async Task CreateChat(ChatGroupView viewModel)
        {

            if (dbContext.ChatGroups.Where(c => c.Name == viewModel.Name).Any())
            {
                // chatName already claimed
                return;
            }

            ChatGroup chatGroup = new ChatGroup(viewModel);
            chatGroup.OwnerId = -1;


            dbContext.ChatGroups.Add(chatGroup);
            await dbContext.SaveChangesAsync();
        }

    }
}
