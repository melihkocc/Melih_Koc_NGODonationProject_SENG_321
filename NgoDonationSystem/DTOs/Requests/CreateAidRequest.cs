using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateAidRequest
    {
        [Display(Name = "Açıklama / Ek Notlar")]
        public string? RequestedItems { get; set; }

        [Required(ErrorMessage = "Talep edilecek ürün seçilmelidir.")]
        [Display(Name = "Stok Ürünü")]
        public int InventoryItemId { get; set; }

        [Required(ErrorMessage = "Miktar belirtilmelidir.")]
        [Display(Name = "Miktar")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Faydalanıcı seçilmelidir.")]
        [Display(Name = "Faydalanıcı")]
        public int BeneficiaryId { get; set; }

        [Display(Name = "Atanan Çalışan / Dağıtıcı")]
        public int? AssignedWorkerId { get; set; }
    }
}
