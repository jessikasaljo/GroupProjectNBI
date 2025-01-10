using Application.Helpers;
using MediatR;
using Application.DTOs.StoreDtos;

namespace Application.Queries.StoreQueries.GetAllStores
{
    public class GetAllStoresQuery : IRequest<OperationResult<IEnumerable<StoreDto>>>
    {
        public int Page { get; set; } = 1;
        public int Hits { get; set; } = 10;
    }
}
