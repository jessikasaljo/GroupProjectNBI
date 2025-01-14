using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public required string Location { get; set; }

        public List<StoreItem> StoreItems { get; set; } = new List<StoreItem>();
    }
}