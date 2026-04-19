using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateApprovalRequest
    {
        [Required(ErrorMessage = "Tür gereklidir.")]
        [Display(Name = "Tür")]
        public string EntityType { get; set; }

        [Required(ErrorMessage = "Entity ID gereklidir.")]
        [Display(Name = "Entity ID")]
        public int EntityId { get; set; }

        [Required(ErrorMessage = "Onaylayacak kişi gereklidir.")]
        [Display(Name = "Onaylayacak Kişi")]
        public int ApproverId { get; set; }

        [Display(Name = "Açıklama")]
        public string Comments { get; set; }
    }
}
