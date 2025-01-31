using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Queries.CartItemQueries.GetCartItemById
{
    public class GetCartItemByIdQuery : IRequest<OperationResult<CartItemDTO>>
    {
        public int Id { get; set; }

        public GetCartItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}
