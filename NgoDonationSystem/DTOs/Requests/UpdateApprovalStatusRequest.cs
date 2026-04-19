using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class UpdateApprovalStatusRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Durum gereklidir.")]
        public string Status { get; set; }

        public string Remarks { get; set; }
    }
}
