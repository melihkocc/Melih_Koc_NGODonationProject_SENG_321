using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateWorkerRequest
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola gereklidir.")]
        [MinLength(6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rol seçimi gereklidir.")]
        [Display(Name = "Rol")]
        public string RoleName { get; set; }
    }
}
