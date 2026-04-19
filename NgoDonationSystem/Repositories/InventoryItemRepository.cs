using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class InventoryItemRepository : Repository<InventoryItem>, IInventoryItemRepository
    {
        public InventoryItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<InventoryItem>> GetAllItemsWithCategoryAsync()
        {
            return await _dbSet.Include(i => i.Category).OrderByDescending(i => i.Id).ToListAsync();
        }

        public async Task<InventoryItem?> GetItemWithCategoryByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Category).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<InventoryItem?> GetItemWithCategoryAndTransactionsByIdAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Category)
                .Include(x => x.Transactions.OrderByDescending(t => t.TransactionDate))
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<InventoryItem>> GetAvailableItemsAsync()
        {
            return await _dbSet.Where(i => i.Quantity > 0).ToListAsync();
        }
    }
}
