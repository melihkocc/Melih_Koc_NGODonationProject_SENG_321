namespace NgoDonationSystem.Models
{
    public class AidRequest
    {
        public int Id { get; set; }
        public int BeneficiaryId { get; set; }
        public int? InventoryItemId { get; set; }
        public decimal Quantity { get; set; }
        public string? RequestedItems { get; set; }
        public string Status { get; set; }
        public int CreatedById { get; set; }

        public Beneficiary Beneficiary { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public User CreatedBy { get; set; }
        
        public int? AssignedWorkerId { get; set; }
        public User AssignedWorker { get; set; }

        public AidDistribution AidDistribution { get; set; }
    }
}
