using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
        public decimal Quantity { get; set; }
        public string UnitType { get; set; }
        public string WarehouseLocation { get; set; }

        public InventoryCategory Category { get; set; }
        public ICollection<InventoryTransaction> Transactions { get; set; }
        public ICollection<AidRequest> AidRequests { get; set; }
    }
}
