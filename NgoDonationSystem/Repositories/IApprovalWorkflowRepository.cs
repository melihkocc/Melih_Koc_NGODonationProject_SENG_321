using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IApprovalWorkflowRepository : IRepository<ApprovalWorkflow>
    {
        Task<List<ApprovalWorkflow>> GetPendingWorkflowsWithDetailsAsync();
    }
}
