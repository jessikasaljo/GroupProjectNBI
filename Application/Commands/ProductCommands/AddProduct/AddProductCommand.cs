using Application.DTOs.Product;
using Application.Helpers;
using MediatR;

namespace Application.Commands.ProductCommands.AddProduct
{
    public class AddProductCommand : IRequest<OperationResult<string>>
    {
        public ProductDTO newProduct { get; set; }
        public AddProductCommand(ProductDTO productToAdd)
        {
            newProduct = productToAdd;
        }
    }
}
