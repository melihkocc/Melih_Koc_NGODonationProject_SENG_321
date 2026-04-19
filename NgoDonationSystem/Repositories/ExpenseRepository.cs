using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Expense>> GetAllExpensesWithDetailsAsync()
        {
            return await _dbSet
                .Include(e => e.Category).Include(e => e.CreatedBy).Include(e => e.ReceiptDocument)
                .OrderByDescending(e => e.ExpenseDate).ToListAsync();
        }

        public async Task<Expense?> GetExpenseWithDetailsByIdAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Category).Include(x => x.CreatedBy).Include(x => x.ReceiptDocument)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<decimal> GetTotalExpensesAmountAsync()
        {
            return await _dbSet.SumAsync(e => e.Amount);
        }
    }
}
