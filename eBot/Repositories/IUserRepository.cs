using System.Threading.Tasks;
using eBot.Data.Domain;

namespace eBot.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(long chatId);

        Task SaveUserAsync(User user);

        Task SaveNewUserAsync(long chatId);
    }
}
