using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class ExpenseResponse
    {
        public int Id { get; set; }

        [Display(Name = "Miktar")]
        public decimal Amount { get; set; }

        [Display(Name = "Kategori")]
        public string CategoryName { get; set; }

        [Display(Name = "Gider Tarihi")]
        public DateTime ExpenseDate { get; set; }

        [Display(Name = "Belge")]
        public string ReceiptFileName { get; set; }

        [Display(Name = "Ekleyen")]
        public string CreatedByName { get; set; }
    }
}

