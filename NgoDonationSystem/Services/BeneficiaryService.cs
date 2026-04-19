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
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _repo;

        public BeneficiaryService(IBeneficiaryRepository repo) { _repo = repo; }

        public async Task<List<BeneficiaryResponse>> GetAllAsync()
        {
            var list = await _repo.GetAllBeneficiariesOrderedAsync();
            return list.Select(b => new BeneficiaryResponse
            {
                Id = b.Id, FullName = $"{b.FirstName} {b.LastName}", Email = b.Email,
                Phone = b.Phone, Address = b.Address, IdentificationNumber = b.IdentificationNumber,
                IsActive = b.IsActive, CreatedAt = b.CreatedAt
            }).ToList();
        }

        public async Task<BeneficiaryResponse?> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return null;
            return new BeneficiaryResponse
            {
                Id = b.Id, FullName = $"{b.FirstName} {b.LastName}", Email = b.Email,
                Phone = b.Phone, Address = b.Address, IdentificationNumber = b.IdentificationNumber,
                IsActive = b.IsActive, CreatedAt = b.CreatedAt
            };
        }

        public async Task CreateAsync(CreateBeneficiaryRequest request)
        {
            var entity = new Beneficiary
            {
                FirstName = request.FirstName, LastName = request.LastName,
                Email = request.Email, Phone = request.Phone,
                Address = request.Address, IdentificationNumber = request.IdentificationNumber,
                IsActive = true, CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
        }

        public async Task<UpdateBeneficiaryRequest?> GetByIdForEditAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return null;
            return new UpdateBeneficiaryRequest
            {
                Id = b.Id, FirstName = b.FirstName, LastName = b.LastName,
                Email = b.Email, Phone = b.Phone, Address = b.Address,
                IdentificationNumber = b.IdentificationNumber, IsActive = b.IsActive
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateBeneficiaryRequest request)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return false;
            b.FirstName = request.FirstName; b.LastName = request.LastName;
            b.Email = request.Email; b.Phone = request.Phone;
            b.Address = request.Address; b.IdentificationNumber = request.IdentificationNumber;
            b.IsActive = request.IsActive;
            await _repo.UpdateAsync(b);
            return true;
        }

        public async Task<BeneficiaryResponse?> GetForDeleteAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return null;
            return new BeneficiaryResponse { Id = b.Id, FullName = $"{b.FirstName} {b.LastName}", IdentificationNumber = b.IdentificationNumber };
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
