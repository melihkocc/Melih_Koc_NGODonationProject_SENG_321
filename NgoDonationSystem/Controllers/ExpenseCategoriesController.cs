using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ExpenseCategoriesController : Controller
    {
        private readonly IExpenseService _service;

        public ExpenseCategoriesController(IExpenseService service)
        {
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();


            return View(await GetCategoryListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetByIdAsync(id.Value);

            return View(response);
        }


        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInventoryCategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateCategoryAsync(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _service.GetCategoryForEditAsync(id.Value);
            if (request == null) return NotFound();
            ViewBag.Id = id.Value;
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateInventoryCategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                var success = await _service.UpdateCategoryAsync(id, request);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Id = id;
            return View(request);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetCategoryForDeleteAsync(id.Value);
            if (response == null) return NotFound();
            return View(response);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<System.Collections.Generic.List<NgoDonationSystem.DTOs.Responses.InventoryCategoryResponse>> GetCategoryListAsync()
        {

            return await _service.GetAllCategoriesAsync();
        }
    }
}
