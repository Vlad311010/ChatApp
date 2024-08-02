using ChatApp.Client.Models;
using ChatApp.Interfaces;

namespace ChatApp.Data
{
    public class UsersRepositoryRuntime : IUsersRepository
    {
        private List<User> users = new List<User>()
        {
            new User("Ruf", "1"),
            new User("Azter", "1"),
            new User("Neider", "1"),
        }; 

        public Task<IEnumerable<User>> All()
        {
            return Task.FromResult<IEnumerable<User>>(users);
        }

        public Task<User?> GetByLogin(string login)
        {
            return Task.FromResult(users.Where(u => u.Login == login).SingleOrDefault());
        }
    }
}
