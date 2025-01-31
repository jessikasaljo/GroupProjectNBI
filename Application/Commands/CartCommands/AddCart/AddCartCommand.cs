using Application.DTOs.CartDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartCommands.AddCart
{
    public class AddCartCommand : IRequest<OperationResult<string>>
    {
        public AddCartDTO NewCart { get; set; }

        public AddCartCommand(AddCartDTO newCart)
        {
            NewCart = newCart;
        }
    }
}
