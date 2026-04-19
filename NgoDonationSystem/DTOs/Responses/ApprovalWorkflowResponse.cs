using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class ApprovalWorkflowResponse
    {
        public int Id { get; set; }

        [Display(Name = "Tür")]
        public string EntityType { get; set; }

        [Display(Name = "Entity Id")]
        public int EntityId { get; set; }

        [Display(Name = "Talep Eden")]
        public string RequestedByName { get; set; }

        [Display(Name = "Onaylayacak Kişi")]
        public string ApproverName { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; }

        [Display(Name = "Açıklama")]
        public string Comments { get; set; }
    }
}
