using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class AidRequestService : IAidRequestService
    {
        private readonly IAidRequestRepository _aidRequestRepo;
        private readonly IBeneficiaryRepository _beneficiaryRepo;
        private readonly IUserRepository _userRepo;
        private readonly IApprovalWorkflowRepository _approvalRepo;
        private readonly IInventoryItemRepository _itemRepo;
        private readonly INotificationService _notificationService;

        public AidRequestService(
            IAidRequestRepository aidRequestRepo,
            IBeneficiaryRepository beneficiaryRepo,
            IUserRepository userRepo,
            IApprovalWorkflowRepository approvalRepo,
            IInventoryItemRepository itemRepo,
            INotificationService notificationService)
        {
            _aidRequestRepo = aidRequestRepo;
            _beneficiaryRepo = beneficiaryRepo;
            _userRepo = userRepo;
            _approvalRepo = approvalRepo;
            _itemRepo = itemRepo;
            _notificationService = notificationService;
        }

        public async Task<List<AidRequestResponse>> GetAllAsync(int? userId = null, string? roleName = null)
        {
            var requests = await _aidRequestRepo.GetAllAidRequestsWithDetailsAsync();

            if (roleName == "FieldWorker" && userId.HasValue)
            {
                requests = requests.Where(a => a.AssignedWorkerId == userId.Value).ToList();
            }

            return requests.Select(a => new AidRequestResponse
            {
                Id = a.Id,
                RequestedItems = a.InventoryItem?.ItemName ?? a.RequestedItems,
                Quantity = a.Quantity,
                Status = a.Status,
                BeneficiaryName = a.Beneficiary != null ? $"{a.Beneficiary.FirstName} {a.Beneficiary.LastName}" : "Bilinmiyor",
                CreatedByName = a.CreatedBy != null ? $"{a.CreatedBy.FirstName} {a.CreatedBy.LastName}" : "Sistem"
            }).ToList();
        }

        public async Task<AidRequestResponse?> GetByIdAsync(int id)
        {
            var aidRequest = await _aidRequestRepo.GetAidRequestWithDetailsByIdAsync(id);

            if (aidRequest == null) return null;

            return new AidRequestResponse
            {
                Id = aidRequest.Id,
                RequestedItems = aidRequest.InventoryItem?.ItemName ?? aidRequest.RequestedItems,
                Status = aidRequest.Status,
                Quantity = aidRequest.Quantity,
                BeneficiaryName = aidRequest.Beneficiary != null ? $"{aidRequest.Beneficiary.FirstName} {aidRequest.Beneficiary.LastName}" : "Bilinmiyor",
                CreatedByName = aidRequest.CreatedBy != null ? $"{aidRequest.CreatedBy.FirstName} {aidRequest.CreatedBy.LastName}" : "Sistem"
            };
        }

        public async Task<(bool success, string? errorMessage)> CreateAsync(CreateAidRequest request, int userId)
        {
            if (request.InventoryItemId > 0)
            {
                var item = await _itemRepo.GetByIdAsync(request.InventoryItemId);
                if (item != null && request.Quantity > item.Quantity)
                {
                    return (false, $"Talep edilen miktar mevcut stoktan ({item.Quantity} {item.UnitType}) fazladır. Hata: Yetersiz Ürün.");
                }
            }

            var aidRequest = new AidRequest
            {
                InventoryItemId = request.InventoryItemId,
                Quantity = request.Quantity,
                RequestedItems = request.RequestedItems,
                BeneficiaryId = request.BeneficiaryId,
                Status = "Pending",
                CreatedById = userId,
                AssignedWorkerId = request.AssignedWorkerId
            };

            await _aidRequestRepo.AddAsync(aidRequest);

            if (request.AssignedWorkerId.HasValue)
            {
                await _notificationService.CreateNotificationAsync(request.AssignedWorkerId.Value, "Yeni Görev Ataması", $"Size yeni bir yardım talebi (#{aidRequest.Id}) atandı.", $"/AidRequests/Details/{aidRequest.Id}");
            }

            var adminUser = await _userRepo.GetAdminUserAsync();

            var approval = new ApprovalWorkflow
            {
                EntityType = "AidRequest",
                EntityId = aidRequest.Id,
                RequestedById = userId,
                ApproverId = adminUser?.Id ?? 1,
                Status = "Pending",
                Comments = $"Yardım Talebi #{aidRequest.Id} için otomatik onay talebi oluşturuldu."
            };

            await _approvalRepo.AddAsync(approval);

            if (adminUser != null)
            {
                await _notificationService.CreateNotificationAsync(adminUser.Id, "Onay Bekleyen İşlem", $"Yardım Talebi #{aidRequest.Id} için onayınız bekleniyor.", "/ApprovalWorkflow/Index");
            }

            return (true, null);
        }

        public async Task<UpdateAidRequest?> GetByIdForEditAsync(int id)
        {
            var aidRequest = await _aidRequestRepo.GetByIdAsync(id);
            if (aidRequest == null) return null;

            return new UpdateAidRequest
            {
                Id = aidRequest.Id,
                InventoryItemId = aidRequest.InventoryItemId ?? 0,
                Quantity = aidRequest.Quantity,
                RequestedItems = aidRequest.RequestedItems,
                Status = aidRequest.Status,
                AssignedWorkerId = aidRequest.AssignedWorkerId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateAidRequest request)
        {
            var aidRequest = await _aidRequestRepo.GetByIdAsync(id);
            if (aidRequest == null) return false;

            aidRequest.InventoryItemId = request.InventoryItemId;
            aidRequest.Quantity = request.Quantity;
            aidRequest.RequestedItems = request.RequestedItems;
            aidRequest.Status = request.Status;
            
            bool newWorkerAssigned = aidRequest.AssignedWorkerId != request.AssignedWorkerId && request.AssignedWorkerId.HasValue;
            aidRequest.AssignedWorkerId = request.AssignedWorkerId;

            await _aidRequestRepo.UpdateAsync(aidRequest);

            if (newWorkerAssigned)
            {
                await _notificationService.CreateNotificationAsync(request.AssignedWorkerId.Value, "Yeni Görev Ataması", $"Size bir yardım talebi (#{aidRequest.Id}) yönlendirildi.", $"/AidRequests/Details/{aidRequest.Id}");
            }

            return true;
        }

        public async Task ApproveAsync(int id)
        {
            var aidRequest = await _aidRequestRepo.GetByIdAsync(id);
            if (aidRequest != null)
            {
                aidRequest.Status = "Approved";
                await _aidRequestRepo.UpdateAsync(aidRequest);
            }
        }

        public async Task<AidRequestResponse?> GetForDeleteAsync(int id)
        {
            var aidRequest = await _aidRequestRepo.GetAidRequestWithDetailsByIdAsync(id);

            if (aidRequest == null) return null;

            return new AidRequestResponse
            {
                Id = aidRequest.Id,
                RequestedItems = aidRequest.InventoryItem?.ItemName ?? aidRequest.RequestedItems,
                Quantity = aidRequest.Quantity,
                Status = aidRequest.Status,
                BeneficiaryName = aidRequest.Beneficiary != null ? $"{aidRequest.Beneficiary.FirstName} {aidRequest.Beneficiary.LastName}" : "Bilinmiyor"
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _aidRequestRepo.DeleteAsync(id);
        }

        public async Task<SelectList> GetBeneficiarySelectListAsync(int? selectedId = null)
        {
            var beneficiaries = await _beneficiaryRepo.GetAllAsync();
            return new SelectList(beneficiaries, "Id", "FirstName", selectedId);
        }

        public async Task<SelectList> GetInventoryItemSelectListAsync(int? selectedId = null)
        {
            var items = await _itemRepo.GetAvailableItemsAsync();
            return new SelectList(items, "Id", "ItemName", selectedId);
        }

        public async Task<SelectList> GetFieldWorkerSelectListAsync(int? selectedId = null)
        {
            var workers = await _userRepo.GetUsersByRoleAsync("FieldWorker");
            return new SelectList(workers, "Id", "FullName", selectedId);
        }
    }
}
