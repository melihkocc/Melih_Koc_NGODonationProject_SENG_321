using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class AidRequestRepository : Repository<AidRequest>, IAidRequestRepository
    {
        public AidRequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<AidRequest>> GetAllAidRequestsWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.Beneficiary)
                .Include(a => a.CreatedBy)
                .Include(a => a.InventoryItem)
                .OrderByDescending(a => a.Id)
                .ToListAsync();
        }

        public async Task<AidRequest?> GetAidRequestWithDetailsByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Beneficiary)
                .Include(a => a.CreatedBy)
                .Include(a => a.InventoryItem)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<AidRequest>> GetApprovedAidRequestsAsync()
        {
            return await _dbSet
                .Include(a => a.InventoryItem)
                .Where(a => a.Status == "Approved")
                .ToListAsync();
        }

        public async Task<int> GetPendingAidRequestsCountAsync()
        {
            return await _dbSet
                .CountAsync(a => a.Status == "Pending" || a.Status == "Submitted" || a.Status == "UnderReview");
        }
    }
}
