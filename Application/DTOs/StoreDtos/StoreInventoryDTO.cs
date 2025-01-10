using Application.DTOs.Product;

namespace Application.DTOs.StoreDtos
{
    public class StoreInventoryDTO
    {
        public string? Location { get; set; }
        public Dictionary<FullProductDTO, int> Inventory { get; set; } = new();
    }
}
