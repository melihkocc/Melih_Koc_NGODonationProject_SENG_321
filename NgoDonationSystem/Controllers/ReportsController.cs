using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using System.Threading.Tasks;

namespace NgoDonationSystem.Controllers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ReportsController : Controller
    {
        private readonly IReportsService _service;

        public ReportsController(IReportsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = await _service.GetReportsDashboardDataAsync();

            var viewModel = new ReportsDashboardViewModel
            {
                Stats = dashboardData,
                DonationSummaries = dashboardData.DonationsByType.Select(s => new DonationSummaryViewModel
                {
                    DonationType = s.DonationType,
                    TotalCount = s.TotalCount,
                    TotalAmount = s.TotalAmount
                }).ToList()
            };

            return View(viewModel);
        }
    }

    public class ReportsDashboardViewModel
    {
        public ReportsDashboardDto Stats { get; set; } = new();
        public List<DonationSummaryViewModel> DonationSummaries { get; set; } = new();
    }

    public class DonationSummaryViewModel
    {
        public string DonationType { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
