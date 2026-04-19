using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DonorType { get; set; }
        public string TaxNumber { get; set; }

        public User User { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }
}
