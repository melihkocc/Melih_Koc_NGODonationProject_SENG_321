using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class BeneficiaryRepository : Repository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Beneficiary>> GetAllBeneficiariesOrderedAsync()
        {
            return await _dbSet.OrderByDescending(b => b.Id).ToListAsync();
        }
    }
}
