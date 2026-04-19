using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IReportsService
    {
        Task<List<DonationSummaryDto>> GetDonationSummaryAsync();
        Task<ReportsDashboardDto> GetReportsDashboardDataAsync();
    }

    public class DonationSummaryDto
    {
        public string DonationType { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ReportsDashboardDto
    {
        public decimal TotalDonationsAmount { get; set; }
        public decimal TotalExpensesAmount { get; set; }
        public int TotalDonorsCount { get; set; }
        public int TotalBeneficiariesCount { get; set; }
        public List<DonationSummaryDto> DonationsByType { get; set; } = new();
        public List<MonthlyTrendDto> MonthlyDonationTrends { get; set; } = new();
        public List<TopDonorDto> TopDonors { get; set; } = new();
        public List<ExpenseSummaryDto> ExpensesByCategory { get; set; } = new();
    }

    public class MonthlyTrendDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class TopDonorDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int DonationCount { get; set; }
    }

    public class ExpenseSummaryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
