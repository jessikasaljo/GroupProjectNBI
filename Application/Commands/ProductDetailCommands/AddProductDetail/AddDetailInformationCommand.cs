using Application.DTOs.Product;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ProductDetailCommands.AddProductDetail
{
    public class AddDetailInformationCommand : IRequest<OperationResult<string>>
    {
        public ProductDetailDTO newProductDetail { get; set; }
        public AddDetailInformationCommand(ProductDetailDTO productDetailToAdd)
        {
            newProductDetail = productDetailToAdd;
        }
    }
}
