using System;

namespace NgoDonationSystem.Models
{
    public class AidDistribution
    {
        public int Id { get; set; }
        public int AidRequestId { get; set; }
        public int DeliveredById { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string DeliveredNotes { get; set; }

        public AidRequest AidRequest { get; set; }
        public User DeliveredBy { get; set; }
    }
}
