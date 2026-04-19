using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class AidDistributionResponse
    {
        public int Id { get; set; }

        [Display(Name = "Talep Edilen Ürünler")]
        public string RequestedItems { get; set; }

        [Display(Name = "Faydalanıcı")]
        public string BeneficiaryName { get; set; }

        [Display(Name = "Teslim Eden")]
        public string DeliveredByName { get; set; }

        [Display(Name = "Teslim Tarihi")]
        public DateTime DeliveredAt { get; set; }

        [Display(Name = "Teslim Notları")]
        public string DeliveredNotes { get; set; }
    }
}
