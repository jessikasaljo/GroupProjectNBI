using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQuery : IRequest<OperationResult<Product?>>
    {
        public int Id { get; set; }
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
