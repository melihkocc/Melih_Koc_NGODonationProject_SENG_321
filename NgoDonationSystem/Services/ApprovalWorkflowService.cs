using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.DTOs.Responses;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class ApprovalWorkflowService : IApprovalWorkflowService
    {
        private readonly IApprovalWorkflowRepository _workflowRepo;
        private readonly IAidRequestRepository _aidReqRepo;
        private readonly IDonationRepository _donationRepo;
        private readonly IUserRepository _userRepo;
        private readonly INotificationService _notificationService;

        public ApprovalWorkflowService(
            IApprovalWorkflowRepository workflowRepo,
            IAidRequestRepository aidReqRepo,
            IDonationRepository donationRepo,
            IUserRepository userRepo,
            INotificationService notificationService)
        {
            _workflowRepo = workflowRepo;
            _aidReqRepo = aidReqRepo;
            _donationRepo = donationRepo;
            _userRepo = userRepo;
            _notificationService = notificationService;
        }

        public async Task<List<ApprovalWorkflowResponse>> GetPendingAsync()
        {
            var workflows = await _workflowRepo.GetPendingWorkflowsWithDetailsAsync();

            return workflows.Select(aw => new ApprovalWorkflowResponse
            {
                Id = aw.Id,
                EntityType = aw.EntityType,
                EntityId = aw.EntityId,
                RequestedByName = aw.RequestedBy != null ? $"{aw.RequestedBy.FirstName} {aw.RequestedBy.LastName}" : "Bilinmiyor",
                ApproverName = aw.Approver != null ? $"{aw.Approver.FirstName} {aw.Approver.LastName}" : "Atanmadı",
                Status = aw.Status,
                Comments = aw.Comments
            }).ToList();
        }

        public async Task<bool> ApproveAsync(int id, int approverId)
        {
            var workflow = await _workflowRepo.GetByIdAsync(id);
            if (workflow == null) return false;

            workflow.Status = "Approved";
            workflow.ApproverId = approverId;
            workflow.Comments = (workflow.Comments ?? "") + " | Admin tarafından onaylandı.";
            await _workflowRepo.UpdateAsync(workflow);

            await UpdateSourceEntityStatus(workflow.EntityType, workflow.EntityId, "Approved");
            return true;
        }

        public async Task<bool> RejectAsync(int id, int approverId, string? rejectReason)
        {
            var workflow = await _workflowRepo.GetByIdAsync(id);
            if (workflow == null) return false;

            workflow.Status = "Rejected";
            workflow.ApproverId = approverId;
            workflow.Comments = string.IsNullOrEmpty(rejectReason)
                ? (workflow.Comments ?? "") + " | Admin tarafından reddedildi."
                : (workflow.Comments ?? "") + $" | Ret sebebi: {rejectReason}";
            await _workflowRepo.UpdateAsync(workflow);

            await UpdateSourceEntityStatus(workflow.EntityType, workflow.EntityId, "Rejected");
            return true;
        }

        private async Task UpdateSourceEntityStatus(string entityType, int entityId, string newStatus)
        {
            switch (entityType)
            {
                case "AidRequest":
                    var aidRequest = await _aidReqRepo.GetByIdAsync(entityId);
                    if (aidRequest != null)
                    {
                        aidRequest.Status = newStatus;
                        await _aidReqRepo.UpdateAsync(aidRequest);

                        if (newStatus == "Approved" && aidRequest.AssignedWorkerId.HasValue)
                        {
                            await _notificationService.CreateNotificationAsync(
                                aidRequest.AssignedWorkerId.Value,
                                "Talep Onaylandı",
                                $"Atandığınız Yardım Talebi (#{aidRequest.Id}) yönetici tarafından onaylandı. Dağıtıma başlayabilirsiniz.",
                                $"/AidRequests/Details/{aidRequest.Id}"
                            );
                        }
                    }
                    break;

                case "Donation":
                    var donation = await _donationRepo.GetByIdAsync(entityId);
                    if (donation != null)
                    {
                        donation.Status = newStatus == "Approved" ? "Completed" : "Rejected";
                        await _donationRepo.UpdateAsync(donation);
                    }
                    break;
                    
                case "User":
                    var user = await _userRepo.GetByIdAsync(entityId);
                    if (user != null)
                    {
                        user.IsActive = newStatus == "Approved";
                        await _userRepo.UpdateAsync(user);
                    }
                    break;
            }
        }
    }
}
