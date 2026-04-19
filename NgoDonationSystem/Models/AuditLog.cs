using System;

namespace NgoDonationSystem.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }

        public User User { get; set; }
    }
}
