using Application.DTOs.CartDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartCommands.AddCart
{
    public class AddCartCommand : IRequest<OperationResult<string>>
    {
        public CartDTO NewCart { get; set; }

        public AddCartCommand(CartDTO newCart)
        {
            NewCart = newCart;
        }
    }
}
