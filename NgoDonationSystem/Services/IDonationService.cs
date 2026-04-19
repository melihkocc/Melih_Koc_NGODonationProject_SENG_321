using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IDonationService
    {
        Task<List<DonationResponse>> GetAllAsync(int userId, bool isDonor);
        Task<UpdateDonationRequest?> GetByIdForEditAsync(int id);
        Task CreateAsync(CreateDonationRequest request, int userId, bool isDonor);
        Task<bool> UpdateAsync(int id, UpdateDonationRequest request);
        Task<SelectList> GetDonorSelectListAsync(int? selectedId = null);
        Task<int?> GetDonorIdByUserIdAsync(int userId);
    }
}
