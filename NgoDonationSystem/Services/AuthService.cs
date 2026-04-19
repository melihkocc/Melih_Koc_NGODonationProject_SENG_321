using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IDonorRepository _donorRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IApprovalWorkflowRepository _approvalRepo;
        private readonly INotificationService _notificationService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepo, 
            IDonorRepository donorRepo, 
            IRoleRepository roleRepo,
            IApprovalWorkflowRepository approvalRepo,
            INotificationService notificationService)
        {
            _userRepo = userRepo;
            _donorRepo = donorRepo;
            _roleRepo = roleRepo;
            _approvalRepo = approvalRepo;
            _notificationService = notificationService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<(User? user, string? roleName, string? error)> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepo.GetUserByEmailWithRoleAsync(email);

            if (user == null) return (null, null, null);

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return (null, null, null);

            if (!user.IsActive)
            {
                return (null, null, "Hesabınız henüz onaylanmadı. Lütfen sistem yöneticisinin onayını bekleyin.");
            }

            return (user, user.Role?.RoleName, null);
        }

        public async Task<bool> RegisterAsync(RegisterUserRequest model)
        {
            if (await EmailExistsAsync(model.Email)) return false;

            var role = await _roleRepo.GetRoleByNameAsync(model.RoleName)
                                ?? await _roleRepo.GetRoleByNameAsync("Donor"); // fallback

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Role = role,
                IsActive = false, // Always requires admin approval now
                CreatedAt = DateTime.UtcNow,
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);
            await _userRepo.AddAsync(newUser);

            if (role?.RoleName == "Donor")
            {
                var newDonor = new Donor
                {
                    UserId = newUser.Id, // We must assign UserId instead of User directly maybe? Both are fine, but EF handles UserId after AddAsync better if we save.
                    DonorType = model.DonorType ?? "Bireysel",
                    TaxNumber = model.TaxNumber ?? "Belirtilmedi"
                };
                newDonor.User = newUser; // Link them
                await _donorRepo.AddAsync(newDonor);
            }

            var adminUser = await _userRepo.GetAdminUserAsync();
            
            var approval = new ApprovalWorkflow
            {
                EntityType = "User",
                EntityId = newUser.Id,
                RequestedById = newUser.Id,
                ApproverId = adminUser?.Id ?? 1,
                Status = "Pending",
                Comments = $"Yeni Sistem Kaydı: {newUser.FirstName} {newUser.LastName} ({role?.RoleName}) — onay bekliyor."
            };
            await _approvalRepo.AddAsync(approval);

            if (adminUser != null)
            {
                await _notificationService.CreateNotificationAsync(
                    adminUser.Id,
                    "Yeni Kullanıcı Kaydı",
                    $"{newUser.FirstName} {newUser.LastName} ({role?.RoleName}) sisteme kayıt oldu ve onayınızı bekliyor.",
                    "/ApprovalWorkflow/Index"
                );
            }

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepo.EmailExistsAsync(email);
        }
    }
}
