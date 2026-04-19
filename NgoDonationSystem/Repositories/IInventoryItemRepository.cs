using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        Task<List<InventoryItem>> GetAllItemsWithCategoryAsync();
        Task<InventoryItem?> GetItemWithCategoryByIdAsync(int id);
        Task<InventoryItem?> GetItemWithCategoryAndTransactionsByIdAsync(int id);
        Task<List<InventoryItem>> GetAvailableItemsAsync();
    }
}
