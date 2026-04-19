using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateAidDistributionRequest
    {
        [Required(ErrorMessage = "Yardım talebi seçilmelidir.")]
        [Display(Name = "Yardım Talebi")]
        public int AidRequestId { get; set; }


        [Display(Name = "Teslim Notları")]
        public string DeliveredNotes { get; set; }
    }
}
