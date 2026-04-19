using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Requests
{
    public class CreateInventoryCategoryRequest
    {
        [Required(ErrorMessage = "Kategori adı gereklidir.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }
    }
}
