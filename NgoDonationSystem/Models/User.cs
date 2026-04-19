using System;
using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public Role Role { get; set; }
        public Donor Donor { get; set; }

        public ICollection<Donation> DonationsCreated { get; set; }
        public ICollection<Expense> ExpensesCreated { get; set; }
        public ICollection<AidRequest> AidRequestsCreated { get; set; }
        public ICollection<AidDistribution> AidDistributionsDelivered { get; set; }
        public ICollection<ApprovalWorkflow> ApprovalWorkflowsRequested { get; set; }
        public ICollection<ApprovalWorkflow> ApprovalWorkflowsApproved { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
        public ICollection<AidRequest> AssignedAidRequests { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
