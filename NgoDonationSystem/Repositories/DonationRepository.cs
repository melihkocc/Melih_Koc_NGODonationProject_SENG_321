using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class DonationRepository : Repository<Donation>, IDonationRepository
    {
        public DonationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Donation>> GetAllDonationsWithDetailsAsync(bool isDonor, int? userId = null)
        {
            var query = _dbSet
                .Include(d => d.Donor).ThenInclude(dr => dr.User)
                .Include(d => d.CreatedBy)
                .AsQueryable();

            if (isDonor && userId.HasValue)
            {
                query = query.Where(d => d.Donor.UserId == userId.Value);
            }

            return await query.OrderByDescending(d => d.Id).ToListAsync();
        }

        public async Task<decimal> GetTotalCompletedDonationsAmountAsync()
        {
            return await _dbSet
                .Where(d => d.Status == "Completed" || d.Status == "Success")
                .SumAsync(d => d.Amount);
        }

        public async Task<List<Donation>> GetRecentDonationsWithUserAsync(int count)
        {
            return await _dbSet
                .Include(d => d.Donor).ThenInclude(dr => dr.User)
                .OrderByDescending(d => d.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<NgoDonationSystem.Services.DonationSummaryDto>> GetDonationSummaryGroupedByTypeAsync()
        {
            return await _dbSet
                .Where(d => d.Status == "Completed" || d.Status == "Success")
                .GroupBy(d => d.DonationType)
                .Select(g => new NgoDonationSystem.Services.DonationSummaryDto
                {
                    DonationType = g.Key,
                    TotalCount = g.Count(),
                    TotalAmount = g.Sum(d => d.Amount)
                })
                .ToListAsync();
        }
    }
}
