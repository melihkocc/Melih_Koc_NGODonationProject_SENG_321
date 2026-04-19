using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IAidDistributionRepository : IRepository<AidDistribution>
    {
        Task<List<AidDistribution>> GetAllDistributionsWithDetailsAsync();
    }
}
