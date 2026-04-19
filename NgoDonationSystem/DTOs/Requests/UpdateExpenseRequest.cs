using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class UpdateExpenseRequest : CreateExpenseRequest
    {
        public int Id { get; set; }
    }
}
