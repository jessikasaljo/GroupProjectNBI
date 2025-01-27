using Application.DTOs.CartItemDtos;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
