using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IDonationRepository _donationRepo;
        private readonly IExpenseRepository _expenseRepo;
        private readonly IDonorRepository _donorRepo;
        private readonly IBeneficiaryRepository _beneficiaryRepo;
        private readonly IAidDistributionRepository _aidRepo;

        public ReportsService(
            IDonationRepository donationRepo,
            IExpenseRepository expenseRepo,
            IDonorRepository donorRepo,
            IBeneficiaryRepository beneficiaryRepo,
            IAidDistributionRepository aidRepo)
        {
            _donationRepo = donationRepo;
            _expenseRepo = expenseRepo;
            _donorRepo = donorRepo;
            _beneficiaryRepo = beneficiaryRepo;
            _aidRepo = aidRepo;
        }

        public async Task<List<DonationSummaryDto>> GetDonationSummaryAsync()
        {
            return await _donationRepo.GetDonationSummaryGroupedByTypeAsync();
        }

        public async Task<ReportsDashboardDto> GetReportsDashboardDataAsync()
        {
            var dashboard = new ReportsDashboardDto();

            // 1. KPI Stats
            dashboard.TotalDonationsAmount = await _donationRepo.GetTotalCompletedDonationsAmountAsync();
            dashboard.TotalExpensesAmount = await _expenseRepo.GetTotalExpensesAmountAsync();
            dashboard.TotalDonorsCount = await _donorRepo.GetTotalDonorsCountAsync();
            var beneficiaries = await _beneficiaryRepo.GetAllAsync();
            dashboard.TotalBeneficiariesCount = beneficiaries.Count();

            // 2. Donations By Type
            dashboard.DonationsByType = await GetDonationSummaryAsync();

            // 3. Monthly Trends (Last 6 months)
            var allDonations = await _donationRepo.GetAllAsync();
            var successfulDonations = allDonations.Where(d => d.Status == "Completed" || d.Status == "Success");

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            dashboard.MonthlyDonationTrends = successfulDonations
                .Where(d => d.CreatedAt >= sixMonthsAgo)
                .GroupBy(d => new { d.CreatedAt.Year, d.CreatedAt.Month })
                .Select(g => new MonthlyTrendDto
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    Amount = g.Sum(d => d.Amount)
                })
                .OrderBy(m => DateTime.ParseExact(m.Month, "MMM yyyy", null))
                .ToList();

            // 4. Top Donors
            var donationsWithDetails = await _donationRepo.GetAllDonationsWithDetailsAsync(false);
            dashboard.TopDonors = donationsWithDetails
                .Where(d => d.Status == "Completed" || d.Status == "Success")
                .GroupBy(d => d.Donor.User.FullName)
                .Select(g => new TopDonorDto
                {
                    Name = g.Key,
                    TotalAmount = g.Sum(d => d.Amount),
                    DonationCount = g.Count()
                })
                .OrderByDescending(d => d.TotalAmount)
                .Take(5)
                .ToList();

            // 5. Expenses By Category
            var expenses = await _expenseRepo.GetAllExpensesWithDetailsAsync();
            dashboard.ExpensesByCategory = expenses
                .GroupBy(e => e.Category.CategoryName)
                .Select(g => new ExpenseSummaryDto
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .OrderByDescending(e => e.TotalAmount)
                .ToList();

            return dashboard;
        }
    }
}
