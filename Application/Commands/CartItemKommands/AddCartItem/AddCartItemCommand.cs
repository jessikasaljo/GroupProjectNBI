using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.CartItemKommands.AddCartItem
{
    public class AddCartItemCommand : IRequest<OperationResult<string>>
    {
        public CartItemDTO NewItem { get; set; }

        public AddCartItemCommand(CartItemDTO newItem)
        {
            NewItem = newItem;
        }

        //public CartItemDTO NewCartItem { get; set; }

        //public AddCartItemCommand(CartItemDTO newCartItem)
        //{
        //    NewCartItem = newCartItem;
        //}
    }
}
