
using System.ComponentModel.DataAnnotations;
namespace Domain.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }


        // Foreign key for Product
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;


        // Quantity of the product in the cart
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
