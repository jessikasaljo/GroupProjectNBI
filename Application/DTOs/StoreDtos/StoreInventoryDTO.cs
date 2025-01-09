using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Product;

namespace Application.DTOs.StoreDtos
{
    public class StoreInventoryDTO
    {
        public string? Location { get; set; }
        public Dictionary<FullProductDTO, int> Inventory { get; set; } = new();
    }
}
