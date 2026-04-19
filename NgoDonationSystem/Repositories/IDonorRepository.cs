using Microsoft.AspNetCore.Mvc.Rendering;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IDonorRepository : IRepository<Donor>
    {
        Task<Donor?> GetDonorByUserIdAsync(int userId);
        Task<List<Donor>> GetAllDonorsWithUserAsync();
        Task<Donor?> GetDonorWithUserByIdAsync(int id);
        Task<int> GetTotalDonorsCountAsync();
    }
}
