using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IBeneficiaryRepository : IRepository<Beneficiary>
    {
        Task<List<Beneficiary>> GetAllBeneficiariesOrderedAsync();
    }
}
