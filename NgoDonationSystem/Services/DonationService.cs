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
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepo;
        private readonly IDonorRepository _donorRepo;
        private readonly IUserRepository _userRepo;
        private readonly IApprovalWorkflowRepository _approvalRepo;

        public DonationService(
            IDonationRepository donationRepo,
            IDonorRepository donorRepo,
            IUserRepository userRepo,
            IApprovalWorkflowRepository approvalRepo)
        {
            _donationRepo = donationRepo;
            _donorRepo = donorRepo;
            _userRepo = userRepo;
            _approvalRepo = approvalRepo;
        }

        public async Task<List<DonationResponse>> GetAllAsync(int userId, bool isDonor)
        {
            var donations = await _donationRepo.GetAllDonationsWithDetailsAsync(isDonor, userId);

            return donations.Select(d => new DonationResponse
            {
                Id = d.Id,
                DonorName = d.Donor?.User != null ? $"{d.Donor.User.FirstName} {d.Donor.User.LastName}" : "Bilinmiyor",
                Amount = d.Amount,
                Currency = d.Currency,
                DonationType = d.DonationType,
                Status = d.Status,
                CreatedByName = d.CreatedBy != null ? $"{d.CreatedBy.FirstName} {d.CreatedBy.LastName}" : "Sistem",
                CreatedAt = d.CreatedAt
            }).ToList();
        }

        public async Task<UpdateDonationRequest?> GetByIdForEditAsync(int id)
        {
            var donation = await _donationRepo.GetByIdAsync(id);
            if (donation == null) return null;

            return new UpdateDonationRequest
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                Amount = donation.Amount,
                Currency = donation.Currency,
                DonationType = donation.DonationType,
                Status = donation.Status
            };
        }

        public async Task CreateAsync(CreateDonationRequest request, int userId, bool isDonor)
        {
            int donorId;

            if (isDonor)
            {
                var donor = await _donorRepo.GetDonorByUserIdAsync(userId);
                if (donor == null) throw new InvalidOperationException("Bağışçı profili bulunamadı.");
                donorId = donor.Id;
            }
            else
            {
                donorId = request.DonorId;
            }

            var needsApproval = true; // Her tutardaki bağış admin onayı gerektirir

            var donation = new Donation
            {
                DonorId = donorId,
                Amount = request.Amount,
                Currency = request.Currency,
                DonationType = request.DonationType,
                Status = "Pending",
                CreatedById = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _donationRepo.AddAsync(donation);

            if (needsApproval)
            {
                var adminUser = await _userRepo.GetAdminUserAsync();

                var approval = new ApprovalWorkflow
                {
                    EntityType = "Donation",
                    EntityId = donation.Id,
                    RequestedById = userId,
                    ApproverId = adminUser?.Id ?? 1,
                    Status = "Pending",
                    Comments = $"Bağış #{donation.Id} ({donation.Amount:N2} {donation.Currency}) — onay bekliyor."
                };

                await _approvalRepo.AddAsync(approval);
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateDonationRequest request)
        {
            var donation = await _donationRepo.GetByIdAsync(id);
            if (donation == null) return false;

            donation.DonorId = request.DonorId;
            donation.Amount = request.Amount;
            donation.Currency = request.Currency;
            donation.DonationType = request.DonationType;
            donation.Status = request.Status;

            await _donationRepo.UpdateAsync(donation);
            return true;
        }

        public async Task<SelectList> GetDonorSelectListAsync(int? selectedId = null)
        {
            var donors = await _donorRepo.GetAllDonorsWithUserAsync();
            return new SelectList(donors, "Id", "User.Email", selectedId);
        }

        public async Task<int?> GetDonorIdByUserIdAsync(int userId)
        {
            var donor = await _donorRepo.GetDonorByUserIdAsync(userId);
            return donor?.Id;
        }
    }
}
