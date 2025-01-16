using Application.DTOs.StoreItemDtos;


namespace Application.DTOs.StoreDtos
{
    public class StoreWithInventoryDTO
    {
        public string? Location { get; set; }
        public List<FullStoreItemDTO> Inventory { get; set; } = new();
    }
}
