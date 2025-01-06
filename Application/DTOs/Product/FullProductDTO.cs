using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dom = Domain.Models;

namespace Application.DTOs.Product
{
    public class FullProductDTO
    {
        public Dom.Product? Product { get; set; }
        public Dom.ProductDetail? ProductDetail { get; set; }
        public FullProductDTO(Dom.Product? productBase, ProductDetail? productDetail)
        {
            Product = productBase;
            ProductDetail = productDetail;
        }
    }
}
