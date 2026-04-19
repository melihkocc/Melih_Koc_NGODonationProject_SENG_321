using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                await _service.MarkAllAsReadAsync(userId);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUnread()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var notifications = await _service.GetUnreadNotificationsAsync(userId);
            return Json(notifications);
        }
    }
}
