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

        //Might modify this later, but made a simple solution to be able to test the Store Crud functionality
        public ICollection<Product> Inventory { get; set; } = new List<Product>();
    }
}