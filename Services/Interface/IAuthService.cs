using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Models;

namespace TicketingSystem.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<bool> ValidateToken(string token);
        Task<bool> DeleteUser(string username);
    }
}
