using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class UpdateBeneficiaryRequest : CreateBeneficiaryRequest
    {
        public int Id { get; set; }

        [Display(Name = "Aktif Mi")]
        public bool IsActive { get; set; }
    }
}
