using System;

namespace NgoDonationSystem.Models
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string TransactionType { get; set; }
        public decimal QuantityChanged { get; set; }
        public DateTime TransactionDate { get; set; }

        public InventoryItem Item { get; set; }
    }
}
