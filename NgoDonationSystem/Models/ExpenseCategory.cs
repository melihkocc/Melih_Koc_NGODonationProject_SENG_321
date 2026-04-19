using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
