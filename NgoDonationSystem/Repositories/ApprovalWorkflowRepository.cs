using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class ApprovalWorkflowRepository : Repository<ApprovalWorkflow>, IApprovalWorkflowRepository
    {
        public ApprovalWorkflowRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ApprovalWorkflow>> GetPendingWorkflowsWithDetailsAsync()
        {
            return await _dbSet
                .Include(aw => aw.RequestedBy)
                .Include(aw => aw.Approver)
                .Where(aw => aw.Status == "Pending")
                .OrderByDescending(aw => aw.Id)
                .ToListAsync();
        }
    }
}
