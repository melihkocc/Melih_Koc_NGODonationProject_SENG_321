using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class AidDistributionService : IAidDistributionService
    {
        private readonly IAidDistributionRepository _distRepo;
        private readonly IAidRequestRepository _aidReqRepo;
        private readonly IInventoryItemRepository _itemRepo;
        private readonly IInventoryTransactionRepository _txRepo;
        private readonly IUserRepository _userRepo;
        private readonly INotificationService _notificationService;

        public AidDistributionService(
            IAidDistributionRepository distRepo,
            IAidRequestRepository aidReqRepo,
            IInventoryItemRepository itemRepo,
            IInventoryTransactionRepository txRepo,
            IUserRepository userRepo,
            INotificationService notificationService)
        {
            _distRepo = distRepo;
            _aidReqRepo = aidReqRepo;
            _itemRepo = itemRepo;
            _txRepo = txRepo;
            _userRepo = userRepo;
            _notificationService = notificationService;
        }

        public async Task<List<AidDistributionResponse>> GetAllAsync(int? userId = null, string? roleName = null)
        {
            var distributions = await _distRepo.GetAllDistributionsWithDetailsAsync();

            if (roleName == "FieldWorker" && userId.HasValue)
            {
                distributions = distributions.Where(d => d.DeliveredById == userId.Value).ToList();
            }

            return distributions.Select(a => new AidDistributionResponse
            {
                Id = a.Id,
                RequestedItems = (a.AidRequest?.Quantity.ToString() ?? "") + "x " + (a.AidRequest?.InventoryItem?.ItemName ?? a.AidRequest?.RequestedItems ?? "Bilinmiyor"),
                BeneficiaryName = a.AidRequest?.Beneficiary != null ? $"{a.AidRequest.Beneficiary.FirstName} {a.AidRequest.Beneficiary.LastName}" : "Bilinmiyor",
                DeliveredByName = a.DeliveredBy != null ? $"{a.DeliveredBy.FirstName} {a.DeliveredBy.LastName}" : "Sistem",
                DeliveredAt = a.DeliveredAt,
                DeliveredNotes = a.DeliveredNotes
            }).ToList();
        }

        public async Task<(bool success, string? errorMessage)> CreateAsync(CreateAidDistributionRequest request, int userId)
        {
            var aidRequest = await _aidReqRepo.GetByIdAsync(request.AidRequestId);
            if (aidRequest == null) return (false, "Yardım talebi bulunamadı.");

            if (!aidRequest.InventoryItemId.HasValue || aidRequest.Quantity <= 0)
            {
                return (false, "Seçilen yardım talebinde stok ürünü eşleştirmesi eksik.");
            }

            var inventoryItem = await _itemRepo.GetByIdAsync(aidRequest.InventoryItemId.Value);
            if (inventoryItem == null || inventoryItem.Quantity < aidRequest.Quantity)
            {
                return (false, "Seçilen ürün için envanterde yeterli stok bulunmamaktadır.");
            }


            inventoryItem.Quantity -= aidRequest.Quantity;
            await _itemRepo.UpdateAsync(inventoryItem);


            var transaction = new InventoryTransaction
            {
                ItemId = aidRequest.InventoryItemId.Value,
                TransactionType = "Out",
                QuantityChanged = -aidRequest.Quantity,
                TransactionDate = DateTime.UtcNow
            };
            await _txRepo.AddAsync(transaction);


            aidRequest.Status = "Teslim Edildi";
            await _aidReqRepo.UpdateAsync(aidRequest);


            var distribution = new AidDistribution
            {
                AidRequestId = request.AidRequestId,
                DeliveredById = userId,
                DeliveredAt = DateTime.UtcNow,
                DeliveredNotes = request.DeliveredNotes
            };
            await _distRepo.AddAsync(distribution);

            var adminUser = await _userRepo.GetAdminUserAsync();
            if (adminUser != null)
            {
                await _notificationService.CreateNotificationAsync(
                    adminUser.Id, 
                    "Yardım Dağıtımı Tamamlandı", 
                    $"Bir yardım talebi (#{aidRequest.Id}) teslim edildi.", 
                    "/AidDistributions/Index"
                );
            }

            return (true, null);
        }

        public async Task<SelectList> GetAidRequestSelectListAsync(int? selectedId = null)
        {
            var approved = await _aidReqRepo.GetApprovedAidRequestsAsync();
            var structuredSelect = approved.Select(a => new {
                Id = a.Id,
                DisplayName = $"{(a.Quantity > 0 ? a.Quantity.ToString() + "x " : "")}{(a.InventoryItem?.ItemName ?? a.RequestedItems ?? "Bilinmiyor")}"
            }).ToList();
            return new SelectList(structuredSelect, "Id", "DisplayName", selectedId);
        }

        public async Task<SelectList> GetUserSelectListAsync(int? selectedId = null)
        {
            var users = await _userRepo.GetAllAsync();
            return new SelectList(users, "Id", "FirstName", selectedId);
        }

        public async Task<SelectList> GetInventoryItemSelectListAsync(int? selectedId = null)
        {
            var items = await _itemRepo.GetAvailableItemsAsync();
            return new SelectList(items, "Id", "ItemName", selectedId);
        }

        public async Task<bool> HasUsersAsync()
        {
            return await _userRepo.HasAnyUsersAsync();
        }
    }
}
