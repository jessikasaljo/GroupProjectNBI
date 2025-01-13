using Application.Helpers;
using MediatR;

namespace Application.Commands.CartCommands.DeleteCart
{
    public class DeleteCartByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }

        public DeleteCartByIdCommand(int id)
        {
            Id = id;
        }
    }
}
