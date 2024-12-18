using Application.Helpers;
using MediatR;

namespace Application.Commands.DeleteAuthor
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
