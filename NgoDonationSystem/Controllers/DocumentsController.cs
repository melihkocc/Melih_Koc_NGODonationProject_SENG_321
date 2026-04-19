using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Models;
using NgoDonationSystem.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,Accountant,FieldWorker")]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _service;
        private readonly IWebHostEnvironment _environment;

        public DocumentsController(IDocumentService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var documents = await _service.GetAllAsync();
            return View(documents);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Lütfen bir dosya seçin.";
                return View();
            }

            try
            {
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

                await _service.AddAsync(document);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Dosya yüklenirken bir hata oluştu: " + ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Download(int id)
        {
            var document = await _service.GetByIdAsync(id);
            if (document == null) return NotFound();

            var filePath = Path.Combine(_environment.WebRootPath, "uploads", document.FilePath);
            if (!System.IO.File.Exists(filePath)) return NotFound("Dosya sunucuda bulunamadı.");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, document.FileType, document.FileName);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var document = await _service.GetByIdAsync(id.Value);
            if (document == null) return NotFound();
            return View(document);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var document = await _service.GetByIdAsync(id.Value);
            if (document == null) return NotFound();
            return View(document);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _service.GetByIdAsync(id);
            if (document != null)
            {

                var filePath = Path.Combine(_environment.WebRootPath, "uploads", document.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                await _service.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
