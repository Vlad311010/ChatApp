using ChatApp.Client.Models;

namespace ChatApp.Data
{
    public class UsersRepository
    {
        private List<User> users = new List<User>()
        {
            new User("Ruf", "1"),
            new User("Azter", "1"),
            new User("Neider", "1"),
        }; 

        public IEnumerable<User> All()
        {
            return users;
        }

        public User? GetByLogin(string login)
        {
            return users.Where(u => u.Login == login).SingleOrDefault();
        }


    }
}
