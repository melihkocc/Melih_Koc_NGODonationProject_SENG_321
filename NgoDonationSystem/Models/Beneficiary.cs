using System;
using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class Beneficiary
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdentificationNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<AidRequest> AidRequests { get; set; }
    }
}
