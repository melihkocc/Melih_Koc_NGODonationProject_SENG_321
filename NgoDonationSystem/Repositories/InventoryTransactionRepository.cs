using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class InventoryTransactionRepository : Repository<InventoryTransaction>, IInventoryTransactionRepository
    {
        public InventoryTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<InventoryTransaction>> GetAllTransactionsWithItemAsync()
        {
            return await _dbSet.Include(t => t.Item).OrderByDescending(t => t.TransactionDate).ToListAsync();
        }
    }
}
