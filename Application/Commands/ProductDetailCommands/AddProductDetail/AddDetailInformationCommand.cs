using Application.DTOs.Product;
using Application.Helpers;
using MediatR;

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
