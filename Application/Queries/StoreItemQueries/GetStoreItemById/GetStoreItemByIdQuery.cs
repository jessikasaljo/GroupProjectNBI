using Application.DTOs.StoreItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Queries.StoreItemQueries.GetStoreItemById
{
    public class GetStoreItemByIdQuery : IRequest<OperationResult<FullStoreItemDTO?>>
    {
        public int Id { get; set; }
        public GetStoreItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}
