using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IAidRequestService
    {
        Task<List<AidRequestResponse>> GetAllAsync(int? userId = null, string? roleName = null);
        Task<AidRequestResponse?> GetByIdAsync(int id);
        Task<(bool success, string? errorMessage)> CreateAsync(CreateAidRequest request, int userId);
        Task<UpdateAidRequest?> GetByIdForEditAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateAidRequest request);
        Task ApproveAsync(int id);
        Task<AidRequestResponse?> GetForDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<SelectList> GetBeneficiarySelectListAsync(int? selectedId = null);
        Task<SelectList> GetInventoryItemSelectListAsync(int? selectedId = null);
        Task<SelectList> GetFieldWorkerSelectListAsync(int? selectedId = null);
    }
}
