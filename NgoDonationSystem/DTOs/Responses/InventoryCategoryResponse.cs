using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class InventoryCategoryResponse
    {
        public int Id { get; set; }

        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }

        public List<InventoryItemResponse> Items { get; set; } = new();
    }
}

