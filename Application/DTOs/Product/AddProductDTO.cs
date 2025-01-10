using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Product
{
    public class AddProductDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public required string Name { get; set; }
    }
}
