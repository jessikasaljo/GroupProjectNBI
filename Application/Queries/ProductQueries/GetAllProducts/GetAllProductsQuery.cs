using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<OperationResult<IEnumerable<Product>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
