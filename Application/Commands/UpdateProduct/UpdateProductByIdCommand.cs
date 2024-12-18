using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.UpdateProduct
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
