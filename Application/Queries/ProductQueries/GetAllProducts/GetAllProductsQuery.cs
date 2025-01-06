using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<OperationResult<IEnumerable<Product>>>
    {
        public int Page { get; set; } = 1;
        public int Hits { get; set; } = 10;
    }
}
