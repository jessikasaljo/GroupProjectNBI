using Domain;
using MediatR;
using Application.Helpers;

namespace Application.Queries.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<OperationResult<Author?>>
    {
        public int Id { get; set; }
        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
