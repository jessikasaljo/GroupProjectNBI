
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        // Calculated field for the total price of the cart
        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.Product.Price * item.Quantity);
            }
        }
    }
}
