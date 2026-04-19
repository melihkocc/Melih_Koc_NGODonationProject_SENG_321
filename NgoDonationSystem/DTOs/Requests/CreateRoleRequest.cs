using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Rol adı gereklidir.")]
        [Display(Name = "Rol Adı")]
        public string RoleName { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
