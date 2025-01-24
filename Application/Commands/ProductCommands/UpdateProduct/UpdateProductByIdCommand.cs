using Application.DTOs.Product;
using Application.Helpers;
using MediatR;

namespace Application.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductByIdCommand : IRequest<OperationResult<string>>
    {
        public ProductDTO UpdatedProduct { get; set; }
        public int Id { get; set; }
        public UpdateProductByIdCommand(ProductDTO updatedProduct, int id)
        {
            UpdatedProduct = updatedProduct;
            Id = id;
        }
    }
}
