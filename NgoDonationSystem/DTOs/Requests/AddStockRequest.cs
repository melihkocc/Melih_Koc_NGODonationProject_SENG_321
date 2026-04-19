using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class AddStockRequest
    {
        [Required]
        public int InventoryItemId { get; set; }

        [Required(ErrorMessage = "Eklenecek miktar belirtilmelidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Miktar sıfırdan büyük olmalıdır.")]
        [Display(Name = "Eklenecek Miktar (Adet/Kg vb.)")]
        public decimal QuantityToAdd { get; set; }
    }
}
