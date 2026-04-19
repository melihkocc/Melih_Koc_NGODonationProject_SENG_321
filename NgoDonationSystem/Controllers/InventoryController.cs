using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,WarehouseStaff,FieldWorker")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            return View(response);
        }

        public async Task<IActionResult> AddStock(int? id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetDetailsAsync(id.Value);
            if (item == null) return NotFound();

            var request = new AddStockRequest
            {
                InventoryItemId = item.Id
            };
            ViewBag.ItemName = item.ItemName;
            ViewBag.CurrentQuantity = item.Quantity;
            ViewBag.UnitType = item.UnitType;
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock(AddStockRequest request)
        {
            if (ModelState.IsValid)
            {
                var success = await _service.AddStockAsync(request);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Stok ekleme işlemi başarısız oldu.");
            }
            
            var item = await _service.GetDetailsAsync(request.InventoryItemId);
            if (item != null)
            {
                ViewBag.ItemName = item.ItemName;
                ViewBag.CurrentQuantity = item.Quantity;
                ViewBag.UnitType = item.UnitType;
            }
            return View(request);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = await _service.GetCategorySelectListAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInventoryItemRequest request)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(request);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
            return View(request);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _service.GetByIdForEditAsync(id.Value);
            if (request == null) return NotFound();
            ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateInventoryItemRequest request)
        {
            if (id != request.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var success = await _service.UpdateAsync(id, request);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
            return View(request);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetDetailsAsync(id.Value);
            if (response == null) return NotFound();
            return View(response);
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
