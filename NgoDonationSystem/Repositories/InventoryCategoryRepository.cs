using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class InventoryCategoryRepository : Repository<InventoryCategory>, IInventoryCategoryRepository
    {
        public InventoryCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<InventoryCategory>> GetAllCategoriesOrderedAsync()
        {
            return await _dbSet.OrderBy(c => c.CategoryName).ToListAsync();
        }

        public async Task<InventoryCategory?> GetCategoryWithItemsByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.InventoryItems).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
