using Application.DTOs.CartDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartCommands.UpdateCart
{
    public class UpdateCartByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public CartDTO UpdatedCart { get; set; }

        public UpdateCartByIdCommand(int id, CartDTO updatedCart)
        {
            Id = id;
            UpdatedCart = updatedCart;
        }
    }
}
