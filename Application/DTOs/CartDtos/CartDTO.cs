using Application.DTOs.CartItemDtos;

namespace Application.DTOs.CartDtos
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
