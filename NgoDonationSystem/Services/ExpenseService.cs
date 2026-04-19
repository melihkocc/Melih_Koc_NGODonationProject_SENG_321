using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.DTOs.Requests;
using NgoDonationSystem.DTOs.Responses;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly IExpenseCategoryRepository _categoryRepo;
        private readonly IDocumentRepository _documentRepo;

        public ExpenseService(IExpenseRepository expenseRepo, IExpenseCategoryRepository categoryRepo, IDocumentRepository documentRepo)
        {
            _expenseRepo = expenseRepo;
            _categoryRepo = categoryRepo;
            _documentRepo = documentRepo;
        }


        public async Task<List<ExpenseResponse>> GetAllAsync()
        {
            var expenses = await _expenseRepo.GetAllExpensesWithDetailsAsync();

            return expenses.Select(e => new ExpenseResponse
            {
                Id = e.Id, Amount = e.Amount,
                CategoryName = e.Category?.CategoryName ?? "Atanmamış",
                ExpenseDate = e.ExpenseDate,
                ReceiptFileName = e.ReceiptDocument?.FileName ?? "Belge Yok",
                CreatedByName = e.CreatedBy?.FirstName ?? "Bilinmiyor"
            }).ToList();
        }

        public async Task<ExpenseResponse?> GetByIdAsync(int id)
        {
            var e = await _expenseRepo.GetExpenseWithDetailsByIdAsync(id);
            if (e == null) return null;
            return new ExpenseResponse
            {
                Id = e.Id, Amount = e.Amount,
                CategoryName = e.Category?.CategoryName ?? "Atanmamış",
                ExpenseDate = e.ExpenseDate,
                ReceiptFileName = e.ReceiptDocument?.FileName ?? "Belge Yok",
                CreatedByName = e.CreatedBy?.FirstName ?? "Bilinmiyor"
            };
        }

        public async Task CreateAsync(CreateExpenseRequest request, int userId)
        {
            var expense = new Expense
            {
                Amount = request.Amount, CategoryId = request.CategoryId,
                ExpenseDate = request.ExpenseDate, ReceiptDocumentId = request.ReceiptDocumentId,
                CreatedById = userId
            };
            await _expenseRepo.AddAsync(expense);
        }

        public async Task<UpdateExpenseRequest?> GetByIdForEditAsync(int id)
        {
            var e = await _expenseRepo.GetByIdAsync(id);
            if (e == null) return null;
            return new UpdateExpenseRequest
            {
                Id = e.Id, Amount = e.Amount, CategoryId = e.CategoryId,
                ExpenseDate = e.ExpenseDate, ReceiptDocumentId = e.ReceiptDocumentId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateExpenseRequest request)
        {
            var e = await _expenseRepo.GetByIdAsync(id);
            if (e == null) return false;
            e.Amount = request.Amount; e.CategoryId = request.CategoryId;
            e.ExpenseDate = request.ExpenseDate; e.ReceiptDocumentId = request.ReceiptDocumentId;
            await _expenseRepo.UpdateAsync(e);
            return true;
        }

        public async Task<ExpenseResponse?> GetForDeleteAsync(int id) => await GetByIdAsync(id);

        public async Task DeleteAsync(int id) => await _expenseRepo.DeleteAsync(id);

        public async Task<SelectList> GetCategorySelectListAsync(int? selectedId = null)
        {
            var cats = await _categoryRepo.GetAllCategoriesOrderedAsync();
            return new SelectList(cats, "Id", "CategoryName", selectedId);
        }

        public async Task<SelectList> GetDocumentSelectListAsync(int? selectedId = null)
        {
            var docs = await _documentRepo.GetAllDocumentsOrderedAsync();
            return new SelectList(docs, "Id", "FileName", selectedId);
        }


        public async Task<List<InventoryCategoryResponse>> GetAllCategoriesAsync()
        {
            var cats = await _categoryRepo.GetAllCategoriesOrderedAsync();
            return cats.Select(c => new InventoryCategoryResponse { Id = c.Id, CategoryName = c.CategoryName }).ToList();
        }

        public async Task<InventoryCategoryResponse?> GetCategoryDetailsAsync(int id)
        {
            var c = await _categoryRepo.GetCategoryWithExpensesByIdAsync(id);
            if (c == null) return null;
            return new InventoryCategoryResponse { Id = c.Id, CategoryName = c.CategoryName };
        }

        public async Task CreateCategoryAsync(CreateInventoryCategoryRequest request)
        {
            await _categoryRepo.AddAsync(new ExpenseCategory { CategoryName = request.CategoryName });
        }

        public async Task<CreateInventoryCategoryRequest?> GetCategoryForEditAsync(int id)
        {
            var c = await _categoryRepo.GetByIdAsync(id);
            if (c == null) return null;
            return new CreateInventoryCategoryRequest { CategoryName = c.CategoryName };
        }

        public async Task<bool> UpdateCategoryAsync(int id, CreateInventoryCategoryRequest request)
        {
            var c = await _categoryRepo.GetByIdAsync(id);
            if (c == null) return false;
            c.CategoryName = request.CategoryName;
            await _categoryRepo.UpdateAsync(c);
            return true;
        }

        public async Task<InventoryCategoryResponse?> GetCategoryForDeleteAsync(int id)
        {
            var c = await _categoryRepo.GetByIdAsync(id);
            if (c == null) return null;
            return new InventoryCategoryResponse { Id = c.Id, CategoryName = c.CategoryName };
        }

        public async Task DeleteCategoryAsync(int id) => await _categoryRepo.DeleteAsync(id);
    }
}
