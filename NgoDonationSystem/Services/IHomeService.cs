using NgoDonationSystem.DTOs.Responses;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IHomeService
    {
        Task<int> GetTotalDonorsAsync();
        Task<decimal> GetTotalFundsAsync();
        Task<int> GetPendingRequestsAsync();
        Task<decimal> GetTotalExpensesAsync();
        Task<List<Donation>> GetRecentDonationsAsync(int count);
    }
}
