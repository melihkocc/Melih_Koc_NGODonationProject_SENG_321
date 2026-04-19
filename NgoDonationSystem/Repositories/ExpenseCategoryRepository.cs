using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class ExpenseCategoryRepository : Repository<ExpenseCategory>, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ExpenseCategory>> GetAllCategoriesOrderedAsync()
        {
            return await _dbSet.OrderBy(c => c.CategoryName).ToListAsync();
        }

        public async Task<ExpenseCategory?> GetCategoryWithExpensesByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Expenses).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
