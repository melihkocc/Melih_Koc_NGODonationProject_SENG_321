using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,FieldWorker")]
    public class AidRequestsController : Controller
    {
        private readonly IAidRequestService _service;

        public AidRequestsController(IAidRequestService service)
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


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetByIdAsync(id.Value);
            if (response == null) return NotFound();
            return View(response);
        }


        public async Task<IActionResult> Create()
        {
            ViewData["BeneficiaryId"] = await _service.GetBeneficiarySelectListAsync();
            ViewData["InventoryItemId"] = await _service.GetInventoryItemSelectListAsync();
            ViewData["AssignedWorkerId"] = await _service.GetFieldWorkerSelectListAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAidRequest request)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
                var userId = int.Parse(userIdString);

                if (User.IsInRole("FieldWorker"))
                {
                    request.AssignedWorkerId = userId;
                }

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
            ViewData["BeneficiaryId"] = await _service.GetBeneficiarySelectListAsync(request.BeneficiaryId);
            ViewData["InventoryItemId"] = await _service.GetInventoryItemSelectListAsync(request.InventoryItemId);
            ViewData["AssignedWorkerId"] = await _service.GetFieldWorkerSelectListAsync(request.AssignedWorkerId);
            return View(request);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _service.GetByIdForEditAsync(id.Value);
            if (request == null) return NotFound();
            ViewData["BeneficiaryId"] = await _service.GetBeneficiarySelectListAsync();
            ViewData["InventoryItemId"] = await _service.GetInventoryItemSelectListAsync(request.InventoryItemId);
            ViewData["AssignedWorkerId"] = await _service.GetFieldWorkerSelectListAsync(request.AssignedWorkerId);
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAidRequest request)
        {
            if (id != request.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var success = await _service.UpdateAsync(id, request);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventoryItemId"] = await _service.GetInventoryItemSelectListAsync(request.InventoryItemId);
            ViewData["AssignedWorkerId"] = await _service.GetFieldWorkerSelectListAsync(request.AssignedWorkerId);
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetForDeleteAsync(id.Value);
            if (response == null) return NotFound();
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
