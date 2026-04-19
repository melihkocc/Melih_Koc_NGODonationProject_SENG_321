using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class AidRequestResponse
    {
        public int Id { get; set; }

        [Display(Name = "Talep Edilen Ürünler")]
        public string RequestedItems { get; set; }

        [Display(Name = "Miktar")]
        public decimal Quantity { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; }

        [Display(Name = "Faydalanıcı")]
        public string BeneficiaryName { get; set; }

        [Display(Name = "Talep Eden")]
        public string CreatedByName { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedAt { get; set; }
    }
}
