using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Queries.GetAuthorById
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
