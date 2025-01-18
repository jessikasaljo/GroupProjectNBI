using Application.DTOs.CartDtos;
using Application.Helpers;
using MediatR;

namespace Application.Queries.CartQuery.GetCartById
{
    public class GetCartByIdQuery : IRequest<OperationResult<CartDTO>>
    {
        public int Id { get; set; }

        public GetCartByIdQuery(int id)
        {
            Id = id;
        }
    }
}
