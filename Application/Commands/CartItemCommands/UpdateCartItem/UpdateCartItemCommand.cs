using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartItemCommands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public UpdateCartItemDTO UpdatedItem { get; set; }

        public UpdateCartItemCommand(int id, UpdateCartItemDTO updatedItem)
        {
            Id = id;
            UpdatedItem = updatedItem;
        }
    }
}
