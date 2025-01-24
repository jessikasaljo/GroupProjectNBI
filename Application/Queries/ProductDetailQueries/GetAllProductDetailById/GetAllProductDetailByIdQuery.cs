using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Queries.ProductDetailQueries.GetProductDetailById
{
    public class GetAllProductDetailByIdQuery : IRequest<OperationResult<IEnumerable<DetailInformation>>>
    {
        public int ProductId { get; set; }
        public GetAllProductDetailByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
