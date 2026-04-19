using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IAidDistributionService
    {
        Task<List<AidDistributionResponse>> GetAllAsync(int? userId = null, string? roleName = null);
        Task<(bool success, string? errorMessage)> CreateAsync(CreateAidDistributionRequest request, int userId);
        Task<SelectList> GetAidRequestSelectListAsync(int? selectedId = null);
        Task<SelectList> GetUserSelectListAsync(int? selectedId = null);
        Task<SelectList> GetInventoryItemSelectListAsync(int? selectedId = null);
        Task<bool> HasUsersAsync();
    }
}
