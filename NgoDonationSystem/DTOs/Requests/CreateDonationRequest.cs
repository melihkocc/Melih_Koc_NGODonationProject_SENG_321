using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateDonationRequest
    {
        [Required(ErrorMessage = "Bağışçı seçilmelidir.")]
        [Display(Name = "Bağışçı")]
        public int DonorId { get; set; }

        [Required(ErrorMessage = "Tutar belirtilmelidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        [Display(Name = "Tutar")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Para birimi belirtilmelidir.")]
        [Display(Name = "Para Birimi")]
        public string Currency { get; set; } = "TRY";

        [Required(ErrorMessage = "Bağış türü belirtilmelidir.")]
        [Display(Name = "Bağış Türü")]
        public string DonationType { get; set; }
    }
}
