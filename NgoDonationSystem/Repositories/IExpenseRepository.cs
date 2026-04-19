using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<List<Expense>> GetAllExpensesWithDetailsAsync();
        Task<Expense?> GetExpenseWithDetailsByIdAsync(int id);
        Task<decimal> GetTotalExpensesAmountAsync();
    }
}
