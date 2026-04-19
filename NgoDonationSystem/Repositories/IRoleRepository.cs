using NgoDonationSystem.Models;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
