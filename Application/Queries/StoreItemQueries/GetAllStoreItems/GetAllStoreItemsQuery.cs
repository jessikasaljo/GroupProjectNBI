using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Queries.StoreItemQueries.GetAllStoreItems
{
    public class GetAllStoreItemsQuery : IRequest<OperationResult<IEnumerable<StoreItem>>>
    {
        public int Page { get; set; } = 1;
        public int Hits { get; set; } = 10;
    }
}
