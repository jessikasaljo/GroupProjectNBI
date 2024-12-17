using Domain;
using MediatR;
using Application.Helpers;

namespace Application.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<OperationResult<IEnumerable<Book>>>
    {
    }
}
