using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductByIdCommand : IRequest<OperationResult<string>>
    {
        public Product UpdatedProduct { get; set; }
        public int Id { get; set; }
        public UpdateProductByIdCommand(Product updatedProduct, int id)
        {
            UpdatedProduct = updatedProduct;
            Id = id;
        }
    }
}
