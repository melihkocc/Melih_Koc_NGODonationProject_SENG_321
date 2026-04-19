using Microsoft.AspNetCore.Identity;
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
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepo;
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly PasswordHasher<User> _passwordHasher;

        public DonorService(
            IDonorRepository donorRepo,
            IUserRepository userRepo,
            IRoleRepository roleRepo)
        {
            _donorRepo = donorRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<List<DonorResponse>> GetAllAsync()
        {
            var donors = await _donorRepo.GetAllDonorsWithUserAsync();

            return donors.Select(d => new DonorResponse
            {
                Id = d.Id,
                UserId = d.UserId,
                FirstName = d.User.FirstName,
                LastName = d.User.LastName,
                Email = d.User.Email,
                DonorType = d.DonorType,
                TaxNumber = d.TaxNumber,
                IsActive = d.User.IsActive,
                CreatedAt = d.User.CreatedAt
            }).ToList();
        }

        public async Task<DonorResponse?> GetByIdAsync(int id)
        {
            var donor = await _donorRepo.GetDonorWithUserByIdAsync(id);

            if (donor == null) return null;

            return new DonorResponse
            {
                Id = donor.Id,
                UserId = donor.UserId,
                FirstName = donor.User.FirstName,
                LastName = donor.User.LastName,
                Email = donor.User.Email,
                DonorType = donor.DonorType,
                TaxNumber = donor.TaxNumber,
                IsActive = donor.User.IsActive,
                CreatedAt = donor.User.CreatedAt
            };
        }

        public async Task<bool> CreateAsync(CreateDonorRequest request)
        {
            var existingUser = await _userRepo.GetUserByEmailWithRoleAsync(request.Email);
            if (existingUser != null) return false;

            var donorRole = await _roleRepo.GetRoleByNameAsync("Donor")
                            ?? new Role { RoleName = "Donor", Description = "Bağışçı Rolü" };

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = donorRole,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            var donor = new Donor
            {
                User = user,
                DonorType = request.DonorType,
                TaxNumber = request.TaxNumber
            };

            await _userRepo.AddAsync(user);
            await _donorRepo.AddAsync(donor);
            return true;
        }

        public async Task<UpdateDonorRequest?> GetByIdForEditAsync(int id)
        {
            var donor = await _donorRepo.GetDonorWithUserByIdAsync(id);

            if (donor == null) return null;

            return new UpdateDonorRequest
            {
                Id = donor.Id,
                FirstName = donor.User.FirstName,
                LastName = donor.User.LastName,
                DonorType = donor.DonorType,
                TaxNumber = donor.TaxNumber
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDonorRequest request)
        {
            var donor = await _donorRepo.GetDonorWithUserByIdAsync(id);

            if (donor == null) return false;

            donor.User.FirstName = request.FirstName;
            donor.User.LastName = request.LastName;
            donor.DonorType = request.DonorType;
            donor.TaxNumber = request.TaxNumber;

            await _donorRepo.UpdateAsync(donor);
            return true;
        }
    }
}
