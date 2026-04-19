using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class HomeService : IHomeService
    {
        private readonly IDonorRepository _donorRepo;
        private readonly IDonationRepository _donationRepo;
        private readonly IAidRequestRepository _aidReqRepo;
        private readonly IExpenseRepository _expenseRepo;

        public HomeService(
            IDonorRepository donorRepo,
            IDonationRepository donationRepo,
            IAidRequestRepository aidReqRepo,
            IExpenseRepository expenseRepo)
        {
            _donorRepo = donorRepo;
            _donationRepo = donationRepo;
            _aidReqRepo = aidReqRepo;
            _expenseRepo = expenseRepo;
        }

        public async Task<int> GetTotalDonorsAsync()
            => await _donorRepo.GetTotalDonorsCountAsync();

        public async Task<decimal> GetTotalFundsAsync()
            => await _donationRepo.GetTotalCompletedDonationsAmountAsync();

        public async Task<int> GetPendingRequestsAsync()
            => await _aidReqRepo.GetPendingAidRequestsCountAsync();

        public async Task<decimal> GetTotalExpensesAsync()
            => await _expenseRepo.GetTotalExpensesAmountAsync();

        public async Task<List<Donation>> GetRecentDonationsAsync(int count)
            => await _donationRepo.GetRecentDonationsWithUserAsync(count);
    }
}
