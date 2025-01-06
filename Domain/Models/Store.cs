using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; }

        public ICollection<Product> Inventory { get; set; } = new List<Product>();

        public Store(string location)
        {
            Location = location;
        }
    }
}