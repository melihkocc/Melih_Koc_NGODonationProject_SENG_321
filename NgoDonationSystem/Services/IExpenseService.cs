using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IExpenseService
    {

        Task<List<ExpenseResponse>> GetAllAsync();
        Task<ExpenseResponse?> GetByIdAsync(int id);
        Task CreateAsync(CreateExpenseRequest request, int userId);
        Task<UpdateExpenseRequest?> GetByIdForEditAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateExpenseRequest request);
        Task<ExpenseResponse?> GetForDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<SelectList> GetCategorySelectListAsync(int? selectedId = null);
        Task<SelectList> GetDocumentSelectListAsync(int? selectedId = null);


        Task<List<InventoryCategoryResponse>> GetAllCategoriesAsync();
        Task<InventoryCategoryResponse?> GetCategoryDetailsAsync(int id);
        Task CreateCategoryAsync(CreateInventoryCategoryRequest request);
        Task<CreateInventoryCategoryRequest?> GetCategoryForEditAsync(int id);
        Task<bool> UpdateCategoryAsync(int id, CreateInventoryCategoryRequest request);
        Task<InventoryCategoryResponse?> GetCategoryForDeleteAsync(int id);
        Task DeleteCategoryAsync(int id);
    }
}
