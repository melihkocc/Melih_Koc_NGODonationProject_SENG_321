using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class AidDistributionRepository : Repository<AidDistribution>, IAidDistributionRepository
    {
        public AidDistributionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<AidDistribution>> GetAllDistributionsWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.AidRequest).ThenInclude(ar => ar!.Beneficiary)
                .Include(a => a.DeliveredBy)
                .OrderByDescending(a => a.Id).ToListAsync();
        }
    }
}
