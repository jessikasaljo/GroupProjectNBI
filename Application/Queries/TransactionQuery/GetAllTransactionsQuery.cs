using Application.DTOs.TransactionDtos;
using Application.Helpers;
using MediatR;


namespace Application.Queries.TransactionQuery
{
    public class GetAllTransactionsQuery : IRequest<OperationResult<IEnumerable<TransactionQueryDTO>>>
    {
        public int Page { get; set; } = 1;
        public int Hits { get; set; } = 10;
    }
}
