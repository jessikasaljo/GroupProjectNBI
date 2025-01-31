using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartItemCommands.AddCartItem
{
    public class AddCartItemCommand : IRequest<OperationResult<string>>
    {
        public AddCartItemDTO NewItem { get; set; }

        public AddCartItemCommand(AddCartItemDTO newItem)
        {
            NewItem = newItem;
        }

    }
}
