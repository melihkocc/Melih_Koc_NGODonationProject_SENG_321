using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IInventoryService
    {
        Task<List<InventoryItemResponse>> GetAllAsync();
        Task<InventoryItemResponse?> GetDetailsAsync(int id);
        Task CreateAsync(CreateInventoryItemRequest request);
        Task<UpdateInventoryItemRequest?> GetByIdForEditAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateInventoryItemRequest request);
        Task<bool> AddStockAsync(AddStockRequest request);
        Task<InventoryItemResponse?> GetForDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<SelectList> GetCategorySelectListAsync(int? selectedId = null);


        Task<List<InventoryCategoryResponse>> GetAllCategoriesAsync();
        Task<InventoryCategoryResponse?> GetCategoryDetailsAsync(int id);
        Task CreateCategoryAsync(CreateInventoryCategoryRequest request);
        Task<CreateInventoryCategoryRequest?> GetCategoryForEditAsync(int id);
        Task<bool> UpdateCategoryAsync(int id, CreateInventoryCategoryRequest request);
        Task<InventoryCategoryResponse?> GetCategoryForDeleteAsync(int id);
        Task DeleteCategoryAsync(int id);


        Task<List<InventoryTransactionResponse>> GetAllTransactionsAsync();
    }
}
