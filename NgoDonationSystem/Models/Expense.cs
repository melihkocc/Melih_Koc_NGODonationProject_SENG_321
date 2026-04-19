using System;

namespace NgoDonationSystem.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int? ReceiptDocumentId { get; set; }
        public int CreatedById { get; set; }

        public ExpenseCategory Category { get; set; }
        public Document ReceiptDocument { get; set; }
        public User CreatedBy { get; set; }
    }
}
