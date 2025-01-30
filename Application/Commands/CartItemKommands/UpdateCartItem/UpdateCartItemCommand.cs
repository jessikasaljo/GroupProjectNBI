using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
