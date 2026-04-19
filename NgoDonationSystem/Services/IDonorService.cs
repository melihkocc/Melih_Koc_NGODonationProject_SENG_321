using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IDonorService
    {
        Task<List<DonorResponse>> GetAllAsync();
        Task<DonorResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateDonorRequest request);
        Task<UpdateDonorRequest?> GetByIdForEditAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateDonorRequest request);
    }
}
