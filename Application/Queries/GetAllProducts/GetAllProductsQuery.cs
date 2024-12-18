using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Queries.GetAllAuthors
{
    public class GetAllProductsQuery : IRequest<OperationResult<IEnumerable<Product>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
