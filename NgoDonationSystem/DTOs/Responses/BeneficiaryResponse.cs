using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class BeneficiaryResponse
    {
        public int Id { get; set; }

        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Display(Name = "Kimlik No")]
        public string IdentificationNumber { get; set; }

        [Display(Name = "Aktif Mi")]
        public bool IsActive { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; }
    }
}
