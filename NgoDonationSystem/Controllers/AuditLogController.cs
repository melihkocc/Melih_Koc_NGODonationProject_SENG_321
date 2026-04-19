using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NgoDonationSystem.Services;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly IAuditLogService _service;

        public AuditLogController(IAuditLogService service)
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
    }
}
