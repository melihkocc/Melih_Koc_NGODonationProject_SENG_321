using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateInventoryItemRequest
    {
        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [Display(Name = "Ürün Adı")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Başlangıç miktarı belirtilmelidir.")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Miktar negatif olamaz.")]
        [Display(Name = "Başlangıç Miktarı")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Birim seçilmelidir.")]
        [Display(Name = "Birim")]
        public string UnitType { get; set; }

        [Display(Name = "Depo Konumu")]
        public string WarehouseLocation { get; set; }
    }
}

