using Application.DTOs.Product;
using Application.Helpers;
using MediatR;

namespace Application.Commands.ProductCommands.AddProduct
{
    public class AddProductCommand : IRequest<OperationResult<string>>
    {
        public AddProductDTO newProduct { get; set; }
        public AddProductCommand(AddProductDTO productToAdd)
        {
            newProduct = productToAdd;
        }
    }
}
