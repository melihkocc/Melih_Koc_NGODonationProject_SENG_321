using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NgoDonationSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Accountant")]
    public class DonationsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DonationsApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations()
        {
            return await _context.Donations.Include(d => d.Donor).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id)
        {
            var donation = await _context.Donations.Include(d => d.Donor).FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound();
            }

            return donation;
        }


        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(Donation donation)
        {
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonation", new { id = donation.Id }, donation);
        }
    }
}
