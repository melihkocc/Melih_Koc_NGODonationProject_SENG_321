using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class InventoryTransactionResponse
    {
        public int Id { get; set; }

        [Display(Name = "Ürün")]
        public string ItemName { get; set; }

        [Display(Name = "İşlem Türü")]
        public string TransactionType { get; set; }

        [Display(Name = "Miktar Değişimi")]
        public decimal QuantityChanged { get; set; }

        [Display(Name = "Tarih")]
        public DateTime TransactionDate { get; set; }
    }
}
