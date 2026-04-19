using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailWithRoleAsync(string email)
        {
            return await _dbSet.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetAdminUserAsync()
        {
            return await _dbSet.Include(u => u.Role).FirstOrDefaultAsync(u => u.Role.RoleName == "Admin" && u.IsActive);
        }

        public async Task<bool> HasAnyUsersAsync()
        {
            return await _dbSet.AnyAsync();
        }

        public async Task<System.Collections.Generic.List<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _dbSet.Include(u => u.Role).Where(u => u.Role.RoleName == roleName && u.IsActive).ToListAsync();
        }
    }
}
