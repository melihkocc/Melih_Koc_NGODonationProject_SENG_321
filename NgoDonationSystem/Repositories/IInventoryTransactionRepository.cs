using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IInventoryTransactionRepository : IRepository<InventoryTransaction>
    {
        Task<List<InventoryTransaction>> GetAllTransactionsWithItemAsync();
    }
}
