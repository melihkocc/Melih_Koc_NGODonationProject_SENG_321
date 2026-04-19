using System;

namespace NgoDonationSystem.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string DonationType { get; set; }
        public string Status { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }

        public Donor Donor { get; set; }
        public User CreatedBy { get; set; }
    }
}
