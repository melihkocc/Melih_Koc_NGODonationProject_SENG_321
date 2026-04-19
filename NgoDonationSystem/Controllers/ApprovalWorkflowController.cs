using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NgoDonationSystem.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApprovalWorkflowController : Controller
    {
        private readonly IApprovalWorkflowService _service;

        public ApprovalWorkflowController(IApprovalWorkflowService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetPendingAsync();
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out var userId);

            var success = await _service.ApproveAsync(id, userId);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string rejectReason)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out var userId);

            var success = await _service.RejectAsync(id, userId, rejectReason);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
