using NgoDonationSystem.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IApprovalWorkflowService
    {
        Task<List<ApprovalWorkflowResponse>> GetPendingAsync();
        Task<bool> ApproveAsync(int id, int approverId);
        Task<bool> RejectAsync(int id, int approverId, string? rejectReason);
    }
}
