using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.DTOs.Requests;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using NgoDonationSystem.Models;
using System;
using System.IO;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _service;
        private readonly IDocumentService _documentService;
        private readonly IWebHostEnvironment _environment;

        public ExpensesController(IExpenseService service, IDocumentService documentService, IWebHostEnvironment environment)
        {
            _service = service;
            _documentService = documentService;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            return View(response);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = await _service.GetCategorySelectListAsync();
            ViewBag.Documents = await _service.GetDocumentSelectListAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExpenseRequest request)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
                var userId = int.Parse(userIdString);

                if (request.UploadedFile != null)
                {
                    try
                    {
                        var docId = await UploadDocumentAsync(request.UploadedFile);
                        if (docId.HasValue)
                        {
                            request.ReceiptDocumentId = docId.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("UploadedFile", "Dosya yüklenirken bir hata oluştu: " + ex.Message);
                        ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
                        ViewBag.Documents = await _service.GetDocumentSelectListAsync(request.ReceiptDocumentId);
                        return View(request);
                    }
                }

                await _service.CreateAsync(request, userId);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
            ViewBag.Documents = await _service.GetDocumentSelectListAsync(request.ReceiptDocumentId);
            return View(request);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _service.GetByIdForEditAsync(id.Value);
            if (request == null) return NotFound();

            ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
            ViewBag.Documents = await _service.GetDocumentSelectListAsync(request.ReceiptDocumentId);
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateExpenseRequest request)
        {
            if (id != request.Id) return NotFound();
            if (ModelState.IsValid)
            {
                if (request.UploadedFile != null)
                {
                    try
                    {
                        var docId = await UploadDocumentAsync(request.UploadedFile);
                        if (docId.HasValue)
                        {
                            request.ReceiptDocumentId = docId.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("UploadedFile", "Dosya yüklenirken bir hata oluştu: " + ex.Message);
                        ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
                        ViewBag.Documents = await _service.GetDocumentSelectListAsync(request.ReceiptDocumentId);
                        return View(request);
                    }
                }

                var success = await _service.UpdateAsync(id, request);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = await _service.GetCategorySelectListAsync(request.CategoryId);
            ViewBag.Documents = await _service.GetDocumentSelectListAsync(request.ReceiptDocumentId);
            return View(request);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _service.GetByIdAsync(id.Value);
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

        private async Task<int?> UploadDocumentAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var document = new Document
            {
                FileName = file.FileName,
                FilePath = uniqueFileName,
                FileType = file.ContentType,
                UploadedDate = DateTime.UtcNow
            };

            await _documentService.AddAsync(document);
            return document.Id;
        }
    }
}
