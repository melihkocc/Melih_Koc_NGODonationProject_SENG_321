using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IDonationRepository : IRepository<Donation>
    {
        Task<List<Donation>> GetAllDonationsWithDetailsAsync(bool isDonor, int? userId = null);
        Task<decimal> GetTotalCompletedDonationsAmountAsync();
        Task<List<Donation>> GetRecentDonationsWithUserAsync(int count);
        Task<List<NgoDonationSystem.Services.DonationSummaryDto>> GetDonationSummaryGroupedByTypeAsync();
    }
}
