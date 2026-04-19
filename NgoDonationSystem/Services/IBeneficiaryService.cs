using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IBeneficiaryService
    {
        Task<List<BeneficiaryResponse>> GetAllAsync();
        Task<BeneficiaryResponse?> GetByIdAsync(int id);
        Task CreateAsync(CreateBeneficiaryRequest request);
        Task<UpdateBeneficiaryRequest?> GetByIdForEditAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateBeneficiaryRequest request);
        Task<BeneficiaryResponse?> GetForDeleteAsync(int id);
        Task DeleteAsync(int id);
    }
}
