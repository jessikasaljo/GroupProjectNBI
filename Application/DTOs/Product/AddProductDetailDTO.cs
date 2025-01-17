using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class AddProductDetailDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid product ID.")]
        public required int ProductId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public required string Title { get; set; }
        [Required(ErrorMessage = "Text is required.")]
        [StringLength(500, ErrorMessage = "Text cannot exceed 500 characters.")]
        public required string Text { get; set; }
    }
}
