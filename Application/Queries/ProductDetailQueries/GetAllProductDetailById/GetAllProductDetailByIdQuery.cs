using Application.Helpers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
