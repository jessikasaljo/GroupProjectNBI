using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;

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
