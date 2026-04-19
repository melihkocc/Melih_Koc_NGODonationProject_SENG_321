using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        public UsersController(IUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllAsync();
            var roles = await _roleRepo.GetAllAsync();
            
            // To display role name easily in the view, we could hydrate properties. 
            // Better to load them. However, GetAllAsync() doesn't include Role. 
            // We can just query them using DbContext but for simplicity, we will fetch users manually or rely on lazy loading if enabled. Actually we can do it via a quick loop.
            foreach(var u in users) {
                var r = roles.FirstOrDefault(x => x.Id == u.RoleId);
                if (r != null) u.Role = r;
            }

            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            var defaultRoles = new[] { "FieldWorker", "WarehouseStaff", "Accountant" };
            var roles = await _roleRepo.GetAllAsync();
            var filteredRoles = roles
                .Where(r => defaultRoles.Contains(r.RoleName))
                .Select(r => new {
                    RoleName = r.RoleName,
                    DisplayName = TranslateRole(r.RoleName)
                }).ToList();

            ViewData["RoleName"] = new SelectList(filteredRoles, "RoleName", "DisplayName");
            return View();
        }

        private string TranslateRole(string roleName)
        {
            switch (roleName)
            {
                case "Admin": return "Admin (Sistem Yöneticisi)";
                case "FieldWorker": return "FieldWorker (Saha Görevlisi / Dağıtıcı)";
                case "WarehouseStaff": return "WarehouseStaff (Depo Sorumlusu)";
                case "Accountant": return "Accountant (Muhasebeci)";
                case "Donor": return "Donor (Bağışçı)";
                default: return roleName;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWorkerRequest request)
        {
            if (ModelState.IsValid)
            {
                if (await _userRepo.EmailExistsAsync(request.Email))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi kullanımda.");
                }
                else
                {
                    var role = await _roleRepo.GetRoleByNameAsync(request.RoleName) ?? new Role { RoleName = request.RoleName, Description = "Personel" };

                    var user = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Role = role,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var hasher = new PasswordHasher<User>();
                    user.PasswordHash = hasher.HashPassword(user, request.Password);

                    await _userRepo.AddAsync(user);

                    return RedirectToAction(nameof(Index));
                }
            }

            var defaultRoles = new[] { "FieldWorker", "WarehouseStaff", "Accountant" };
            var roles = await _roleRepo.GetAllAsync();
            var filteredRoles = roles
                .Where(r => defaultRoles.Contains(r.RoleName))
                .Select(r => new {
                    RoleName = r.RoleName,
                    DisplayName = TranslateRole(r.RoleName)
                }).ToList();

            ViewData["RoleName"] = new SelectList(filteredRoles, "RoleName", "DisplayName", request.RoleName);
            return View(request);
        }
    }
}
