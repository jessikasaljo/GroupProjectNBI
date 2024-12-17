using Domain;
using MediatR;
using Application.Helpers;

namespace Application.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<OperationResult<Book?>>
    {
        public int Id { get; set; }
        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
    }
}
