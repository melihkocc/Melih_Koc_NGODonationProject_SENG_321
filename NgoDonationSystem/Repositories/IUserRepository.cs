using NgoDonationSystem.Models;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailWithRoleAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetAdminUserAsync();
        Task<System.Collections.Generic.List<User>> GetUsersByRoleAsync(string roleName);
        Task<bool> HasAnyUsersAsync();
    }
}
