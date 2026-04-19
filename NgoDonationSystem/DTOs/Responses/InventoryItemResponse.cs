using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class InventoryItemResponse
    {
        public int Id { get; set; }

        [Display(Name = "Ürün Adı")]
        public string ItemName { get; set; }

        [Display(Name = "Kategori")]
        public string CategoryName { get; set; }

        [Display(Name = "Miktar")]
        public decimal Quantity { get; set; }

        [Display(Name = "Birim")]
        public string UnitType { get; set; }

        [Display(Name = "Depo Konumu")]
        public string WarehouseLocation { get; set; }

        public List<InventoryTransactionResponse> Transactions { get; set; } = new();
    }
}

