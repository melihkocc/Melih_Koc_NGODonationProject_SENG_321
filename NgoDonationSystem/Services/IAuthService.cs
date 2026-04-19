using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IAuthService
    {
        Task<(User? user, string? roleName, string? error)> AuthenticateAsync(string email, string password);
        Task<bool> RegisterAsync(RegisterUserRequest model);
        Task<bool> EmailExistsAsync(string email);
    }
}
