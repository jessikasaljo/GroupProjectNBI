using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.TransactionDtos;
using Application.Helpers;


namespace Application.Queries.TransactionQuery
{
    public class GetAllTransactionsQuery : IRequest<OperationResult<IEnumerable<TransactionQueryDTO>>>
    {
        public int Page { get; set; } = 0;
        public int Hits { get; set; } = 10;
    }
}
