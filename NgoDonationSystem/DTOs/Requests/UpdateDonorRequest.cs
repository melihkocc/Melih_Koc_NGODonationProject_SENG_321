using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class UpdateDonorRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Bağışçı türü gereklidir.")]
        [Display(Name = "Bağışçı Türü")]
        public string DonorType { get; set; }

        [Required(ErrorMessage = "Vergi Numarası gereklidir.")]
        [Display(Name = "Vergi Numarası")]
        public string TaxNumber { get; set; }
    }
}
