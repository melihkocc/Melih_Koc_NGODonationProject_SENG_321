using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class InventoryCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
