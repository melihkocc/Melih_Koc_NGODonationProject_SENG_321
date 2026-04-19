using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,Accountant,Donor")]
    public class DonationsController : Controller
    {
        private readonly IDonationService _service;

        public DonationsController(IDonationService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var userId = int.Parse(userIdString);
            var isDonor = User.IsInRole("Donor");

            var response = await _service.GetAllAsync(userId, isDonor);
            return View(response);
        }


        public async Task<IActionResult> Create()
        {
            if (!User.IsInRole("Donor"))
            {
                ViewBag.DonorId = await _service.GetDonorSelectListAsync();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDonationRequest request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var userId = int.Parse(userIdString);
            var isDonor = User.IsInRole("Donor");

            if (ModelState.IsValid || isDonor)
            {
                try
                {
                    await _service.CreateAsync(request, userId, isDonor);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if (!isDonor)
            {
                ViewBag.DonorId = await _service.GetDonorSelectListAsync(request.DonorId);
            }
            return View(request);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var request = await _service.GetByIdForEditAsync(id.Value);
            if (request == null) return NotFound();

            ViewBag.DonorId = await _service.GetDonorSelectListAsync(request.DonorId);
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDonationRequest request)
        {
            if (id != request.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _service.UpdateAsync(id, request);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DonorId = await _service.GetDonorSelectListAsync(request.DonorId);
            return View(request);
        }
    }
}
