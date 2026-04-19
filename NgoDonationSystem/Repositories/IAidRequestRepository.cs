using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IAidRequestRepository : IRepository<AidRequest>
    {
        Task<List<AidRequest>> GetAllAidRequestsWithDetailsAsync();
        Task<AidRequest?> GetAidRequestWithDetailsByIdAsync(int id);
        Task<List<AidRequest>> GetApprovedAidRequestsAsync();
        Task<int> GetPendingAidRequestsCountAsync();
    }
}
