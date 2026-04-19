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
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryItemRepository _itemRepo;
        private readonly IInventoryCategoryRepository _catRepo;
        private readonly IInventoryTransactionRepository _txRepo;

        public InventoryService(IInventoryItemRepository itemRepo, IInventoryCategoryRepository catRepo, IInventoryTransactionRepository txRepo)
        {
            _itemRepo = itemRepo;
            _catRepo = catRepo;
            _txRepo = txRepo;
        }


        public async Task<List<InventoryItemResponse>> GetAllAsync()
        {
            var items = await _itemRepo.GetAllItemsWithCategoryAsync();
            return items.Select(i => new InventoryItemResponse
            {
                Id = i.Id, ItemName = i.ItemName, CategoryName = i.Category?.CategoryName ?? "Kategorisiz",
                Quantity = i.Quantity, UnitType = i.UnitType, WarehouseLocation = i.WarehouseLocation
            }).ToList();
        }

        public async Task<InventoryItemResponse?> GetDetailsAsync(int id)
        {
            var i = await _itemRepo.GetItemWithCategoryAndTransactionsByIdAsync(id);
            if (i == null) return null;
            return new InventoryItemResponse
            {
                Id = i.Id, ItemName = i.ItemName, CategoryName = i.Category?.CategoryName ?? "Kategorisiz",
                Quantity = i.Quantity, UnitType = i.UnitType, WarehouseLocation = i.WarehouseLocation,
                Transactions = i.Transactions.Select(t => new InventoryTransactionResponse
                {
                    Id = t.Id, ItemName = i.ItemName,
                    TransactionType = t.TransactionType == "In" ? "Giriş" : (t.TransactionType == "Out" ? "Çıkış" : t.TransactionType),
                    QuantityChanged = t.QuantityChanged, TransactionDate = t.TransactionDate
                }).ToList()
            };
        }

        public async Task CreateAsync(CreateInventoryItemRequest request)
        {
            var item = new InventoryItem
            {
                ItemName = request.ItemName, CategoryId = request.CategoryId,
                Quantity = request.Quantity, UnitType = request.UnitType,
                WarehouseLocation = request.WarehouseLocation
            };
            await _itemRepo.AddAsync(item);
        }

        public async Task<UpdateInventoryItemRequest?> GetByIdForEditAsync(int id)
        {
            var i = await _itemRepo.GetByIdAsync(id);
            if (i == null) return null;
            return new UpdateInventoryItemRequest
            {
                Id = i.Id, ItemName = i.ItemName, CategoryId = i.CategoryId,
                Quantity = i.Quantity, UnitType = i.UnitType, WarehouseLocation = i.WarehouseLocation
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateInventoryItemRequest request)
        {
            var i = await _itemRepo.GetByIdAsync(id);
            if (i == null) return false;
            i.ItemName = request.ItemName; i.CategoryId = request.CategoryId;
            i.Quantity = request.Quantity; i.UnitType = request.UnitType;
            i.WarehouseLocation = request.WarehouseLocation;
            await _itemRepo.UpdateAsync(i);
            return true;
        }

        public async Task<bool> AddStockAsync(AddStockRequest request)
        {
            var item = await _itemRepo.GetByIdAsync(request.InventoryItemId);
            if (item == null) return false;

            item.Quantity += request.QuantityToAdd;
            await _itemRepo.UpdateAsync(item);

            var transaction = new InventoryTransaction
            {
                ItemId = item.Id,
                TransactionType = "In",
                QuantityChanged = request.QuantityToAdd,
                TransactionDate = System.DateTime.UtcNow
            };
            await _txRepo.AddAsync(transaction);

            return true;
        }

        public async Task<InventoryItemResponse?> GetForDeleteAsync(int id)
        {
            var i = await _itemRepo.GetItemWithCategoryByIdAsync(id);
            if (i == null) return null;
            return new InventoryItemResponse
            {
                Id = i.Id, ItemName = i.ItemName, CategoryName = i.Category?.CategoryName ?? "Kategorisiz",
                Quantity = i.Quantity, UnitType = i.UnitType, WarehouseLocation = i.WarehouseLocation
            };
        }

        public async Task DeleteAsync(int id) => await _itemRepo.DeleteAsync(id);

        public async Task<SelectList> GetCategorySelectListAsync(int? selectedId = null)
        {
            var cats = await _catRepo.GetAllAsync();
            return new SelectList(cats, "Id", "CategoryName", selectedId);
        }


        public async Task<List<InventoryCategoryResponse>> GetAllCategoriesAsync()
        {
            var cats = await _catRepo.GetAllCategoriesOrderedAsync();
            return cats.Select(c => new InventoryCategoryResponse { Id = c.Id, CategoryName = c.CategoryName }).ToList();
        }

        public async Task<InventoryCategoryResponse?> GetCategoryDetailsAsync(int id)
        {
            var c = await _catRepo.GetCategoryWithItemsByIdAsync(id);
            if (c == null) return null;
            return new InventoryCategoryResponse
            {
                Id = c.Id, CategoryName = c.CategoryName,
                Items = c.InventoryItems.Select(i => new InventoryItemResponse
                {
                    Id = i.Id, ItemName = i.ItemName, Quantity = i.Quantity,
                    UnitType = i.UnitType, WarehouseLocation = i.WarehouseLocation
                }).ToList()
            };
        }

        public async Task CreateCategoryAsync(CreateInventoryCategoryRequest request)
        {
            await _catRepo.AddAsync(new InventoryCategory { CategoryName = request.CategoryName });
        }

        public async Task<CreateInventoryCategoryRequest?> GetCategoryForEditAsync(int id)
        {
            var c = await _catRepo.GetByIdAsync(id);
            if (c == null) return null;
            return new CreateInventoryCategoryRequest { CategoryName = c.CategoryName };
        }

        public async Task<bool> UpdateCategoryAsync(int id, CreateInventoryCategoryRequest request)
        {
            var c = await _catRepo.GetByIdAsync(id);
            if (c == null) return false;
            c.CategoryName = request.CategoryName;
            await _catRepo.UpdateAsync(c);
            return true;
        }

        public async Task<InventoryCategoryResponse?> GetCategoryForDeleteAsync(int id)
        {
            var c = await _catRepo.GetByIdAsync(id);
            if (c == null) return null;
            return new InventoryCategoryResponse { Id = c.Id, CategoryName = c.CategoryName };
        }

        public async Task DeleteCategoryAsync(int id) => await _catRepo.DeleteAsync(id);


        public async Task<List<InventoryTransactionResponse>> GetAllTransactionsAsync()
        {
            var txs = await _txRepo.GetAllTransactionsWithItemAsync();
            return txs.Select(t => new InventoryTransactionResponse
            {
                Id = t.Id, ItemName = t.Item?.ItemName ?? "Bilinmiyor",
                TransactionType = t.TransactionType == "In" ? "Giriş" : (t.TransactionType == "Out" ? "Çıkış" : t.TransactionType),
                QuantityChanged = t.QuantityChanged, TransactionDate = t.TransactionDate
            }).ToList();
        }
    }
}
