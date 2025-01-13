
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<CartItem> Items { get; set; } = new();

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
