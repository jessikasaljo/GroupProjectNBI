using Application.Helpers;
using MediatR;

namespace Application.Commands.CartItemCommands.DeleteCartItem
{
    public class DeleteCartItemCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }

        public DeleteCartItemCommand(int id)
        {
            Id = id;
        }
    }
}
