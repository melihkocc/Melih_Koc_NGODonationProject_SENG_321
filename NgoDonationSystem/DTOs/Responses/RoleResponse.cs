using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class RoleResponse
    {
        public int Id { get; set; }

        [Display(Name = "Rol Adı")]
        public string RoleName { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
