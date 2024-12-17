using Domain;
using MediatR;
using Application.Helpers;

namespace Application.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<OperationResult<IEnumerable<Author>>>
    {
    }
}
