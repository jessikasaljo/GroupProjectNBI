using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartItemKommands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public CartItemDTO UpdatedItem { get; set; }

        public UpdateCartItemCommand(int id, CartItemDTO updatedItem)
        {
            Id = id;
            UpdatedItem = updatedItem;
        }
    }
}
