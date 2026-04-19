using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen bir rol seçiniz.")]
        [Display(Name = "Kayıt Olmak İstenen Rol (Görev)")]
        public string RoleName { get; set; }

        [Display(Name = "Bağışçı Türü (Bireysel/Kurumsal)")]
        public string? DonorType { get; set; }

        [Display(Name = "Vergi Numarası / TCKN")]
        public string? TaxNumber { get; set; }
    }
}
