using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class DonorResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Bağışçı Türü")]
        public string DonorType { get; set; }

        [Display(Name = "Vergi Numarası")]
        public string TaxNumber { get; set; }

        [Display(Name = "Aktif Mi")]
        public bool IsActive { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public System.DateTime CreatedAt { get; set; }
    }
}
