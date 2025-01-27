using Application.DTOs.CartDtos;
using Application.Helpers;
using MediatR;

namespace Application.Queries.CartQuery.GetAllCarts
{
    public class GetAllCartsQuery : IRequest<OperationResult<IEnumerable<CartDTO>>>
    {
        public int Page { get; set; } = 1;
        public int Hits { get; set; } = 10;
    }
}
