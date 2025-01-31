using Application.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.StoreItemDtos
{
    public class FullStoreItemDTO
    {
        public FullProductDTO Product { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        public string StoreLocation { get; set; } = string.Empty;
    }
}
