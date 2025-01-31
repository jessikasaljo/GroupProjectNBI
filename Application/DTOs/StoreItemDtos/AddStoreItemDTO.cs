using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.StoreItemDtos
{
    public class AddStoreItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public int StoreId { get; set; }
    }
}