using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateExpenseRequest
    {
        [Required(ErrorMessage = "Miktar belirtilmelidir.")]
        [Display(Name = "Miktar")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Kategori belirtilmelidir.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Gider Tarihi belirtilmelidir.")]
        [Display(Name = "Gider Tarihi")]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        public int? ReceiptDocumentId { get; set; }

        [Display(Name = "Yeni Dosya Yükle")]
        public IFormFile? UploadedFile { get; set; }
    }
}
