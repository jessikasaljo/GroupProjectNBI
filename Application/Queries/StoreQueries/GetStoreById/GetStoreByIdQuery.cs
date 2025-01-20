using Application.DTOs.StoreDtos;
using Application.Helpers;
using MediatR;

namespace Application.Queries.StoreQueries.GetStoreById
{
    public class GetStoreByIdQuery : IRequest<OperationResult<StoreWithInventoryDTO>>
    {
        public int Id { get; set; }
    }
}