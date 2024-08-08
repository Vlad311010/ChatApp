using ChatApp.Client.ApiUtils;
using ChatApp.Client.Models;

namespace ChatApp.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> All();
        Task<User?> GetByLogin(string login);
        Task<BooleanResponce> Create(UserData user);
    }
}
