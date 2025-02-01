using Domain.Models;

namespace Application.DTOs.TransactionDtos
{
    public class TransactionQueryDTO
    {
        public required string UserName { get; set; }
        public int storeId { get; set; }
        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
        public DateTime TransactionDate { get; set; }
    }
}
