using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,WarehouseStaff")]
    public class InventoryTransactionsController : Controller
    {
        private readonly IInventoryService _service;

        public InventoryTransactionsController(IInventoryService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllTransactionsAsync();
            return View(response);
        }
    }
}
