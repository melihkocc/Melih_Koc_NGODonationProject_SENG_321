using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateBeneficiaryRequest
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Geçersiz e-posta.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçersiz telefon.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Adres gereklidir.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Kimlik numarası gereklidir.")]
        [Display(Name = "Kimlik / Pasaport No")]
        public string IdentificationNumber { get; set; }
    }
}
