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

        //Key value pair where the key is the product and the value is the quantity
        public List<KeyValuePair<Product, int>> Inventory { get; set; } = new List<KeyValuePair<Product, int>>();
    }
}