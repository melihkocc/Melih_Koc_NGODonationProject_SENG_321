using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class DonorRepository : Repository<Donor>, IDonorRepository
    {
        public DonorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Donor?> GetDonorByUserIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<List<Donor>> GetAllDonorsWithUserAsync()
        {
            return await _dbSet.Include(d => d.User).OrderByDescending(d => d.Id).ToListAsync();
        }

        public async Task<Donor?> GetDonorWithUserByIdAsync(int id)
        {
            return await _dbSet.Include(d => d.User).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetTotalDonorsCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
