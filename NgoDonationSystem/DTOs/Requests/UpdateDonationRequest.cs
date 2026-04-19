using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class UpdateDonationRequest : CreateDonationRequest
    {
        public int Id { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; }
    }
}
