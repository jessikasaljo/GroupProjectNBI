using Application.Helpers;
using MediatR;

namespace Application.Commands.ProductCommands.DeleteProduct
{
    public class DeleteProductByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public DeleteProductByIdCommand(int id)
        {
            Id = id;
        }
    }
}
