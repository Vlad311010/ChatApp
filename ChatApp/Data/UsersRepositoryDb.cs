using ChatApp.Client.Models;
using ChatApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class UsersRepositoryDb : IUsersRepository
    {
        public readonly AppDbContext dbContext;
        public UsersRepositoryDb(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> All()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByLogin(string login)
        {
            return await dbContext.Users.Where(u => u.Login == login).SingleOrDefaultAsync();
        }

    }
}
