using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IExpenseCategoryRepository : IRepository<ExpenseCategory>
    {
        Task<List<ExpenseCategory>> GetAllCategoriesOrderedAsync();
        Task<ExpenseCategory?> GetCategoryWithExpensesByIdAsync(int id);
    }
}
