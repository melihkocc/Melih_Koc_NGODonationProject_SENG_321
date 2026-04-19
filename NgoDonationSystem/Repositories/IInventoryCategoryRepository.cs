using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IInventoryCategoryRepository : IRepository<InventoryCategory>
    {
        Task<List<InventoryCategory>> GetAllCategoriesOrderedAsync();
        Task<InventoryCategory?> GetCategoryWithItemsByIdAsync(int id);
    }
}
