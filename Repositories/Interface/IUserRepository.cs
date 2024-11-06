using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task CreateAsync(User user);
        Task DeleteAsync(User user);
    }
}
