
using System.ComponentModel.DataAnnotations;
namespace Domain.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for User
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // List of products in the cart with quantity
        public List<CartItem> Items { get; set; } = new();

    }
}
