using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class UpdateApprovalRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Durum gereklidir.")]
        [Display(Name = "Durum")]
        public string Status { get; set; }

        [Display(Name = "Açıklama")]
        public string Comments { get; set; }
    }
}
