using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CartItemQueries.GetAllCartItems
{
    public class GetAllCartItemsQuery : IRequest<OperationResult<IEnumerable<CartItemDTO>>>
    {
        public int CartId { get; set; }

        public GetAllCartItemsQuery(int cartId)
        {
            CartId = cartId;
        }
    }
}
