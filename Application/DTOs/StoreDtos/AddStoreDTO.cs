using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.StoreDtos
{
    public class AddStoreDTO
    {
        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public required string Location { get; set; }
    }
}
