using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class DonationResponse
    {
        public int Id { get; set; }

        [Display(Name = "Bağışçı")]
        public string DonorName { get; set; }

        [Display(Name = "Miktar")]
        public decimal Amount { get; set; }

        [Display(Name = "Para Birimi")]
        public string Currency { get; set; }

        [Display(Name = "Bağış Türü")]
        public string DonationType { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; }

        [Display(Name = "İşlem Yapan")]
        public string CreatedByName { get; set; }

        [Display(Name = "Tarih")]
        public DateTime CreatedAt { get; set; }
    }
}
