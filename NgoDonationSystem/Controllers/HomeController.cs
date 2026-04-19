using Microsoft.AspNetCore.Mvc;
using NgoDonationSystem.Services;
using NgoDonationSystem.Models;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace NgoDonationSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _service;

        public HomeController(IHomeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalDonors = await _service.GetTotalDonorsAsync();
            ViewBag.TotalFunds = await _service.GetTotalFundsAsync();
            ViewBag.PendingRequests = await _service.GetPendingRequestsAsync();
            ViewBag.TotalExpenses = await _service.GetTotalExpensesAsync();

            var recentDonations = await _service.GetRecentDonationsAsync(5);
            return View(recentDonations);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
