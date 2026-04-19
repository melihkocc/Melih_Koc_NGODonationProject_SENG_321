namespace NgoDonationSystem.Models
{
    public class ApprovalWorkflow
    {
        public int Id { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public int RequestedById { get; set; }
        public int ApproverId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }

        public User RequestedBy { get; set; }
        public User Approver { get; set; }
    }
}
