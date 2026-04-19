using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,FieldWorker")]
    public class AidDistributionsController : Controller
    {
        private readonly IAidDistributionService _service;

        public AidDistributionsController(IAidDistributionService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? userId = string.IsNullOrEmpty(userIdString) ? (int?)null : int.Parse(userIdString);
            string? roleName = User.FindFirstValue(ClaimTypes.Role);

            var response = await _service.GetAllAsync(userId, roleName);
            return View(response);
        }


        public async Task<IActionResult> Create()
        {
            if (!await _service.HasUsersAsync())
            {
                ViewBag.WarningMessage = "Sistemde kayıtlı kullanıcı bulunamadı.";
            }

            ViewBag.AidRequestId = await _service.GetAidRequestSelectListAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAidDistributionRequest request)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
                var userId = int.Parse(userIdString);

                var (success, errorMessage) = await _service.CreateAsync(request, userId);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, errorMessage ?? "Bir hata oluştu.");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.AidRequestId = await _service.GetAidRequestSelectListAsync(request.AidRequestId);
            return View(request);
        }
    }
}
