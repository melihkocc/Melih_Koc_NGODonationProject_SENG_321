using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class DonorsController : Controller
    {
        private readonly IDonorService _service;

        public DonorsController(IDonorService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            return View(response);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetByIdAsync(id.Value);
            if (response == null) return NotFound();
            return View(response);
        }


        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDonorRequest request)
        {
            if (ModelState.IsValid)
            {
                var success = await _service.CreateAsync(request);
                if (!success)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanımda.");
                    return View(request);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _service.GetByIdForEditAsync(id.Value);
            if (request == null) return NotFound();
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDonorRequest request)
        {
            if (id != request.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var success = await _service.UpdateAsync(id, request);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }
    }
}
