using ChatApp.Client.ApiUtils;
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

        public async Task<BooleanResponce> Create(UserData user)
        {
            if (dbContext.Users.Any(u => u.Login == user.Login))
            {
                return new BooleanResponce(false, $"User with login '{user.Login}' already exists");
            }

            
            dbContext.Users.Add(new User(user));
            await dbContext.SaveChangesAsync();
            return new BooleanResponce(true);
        }

    }
}
