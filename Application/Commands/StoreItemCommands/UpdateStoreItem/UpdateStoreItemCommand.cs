using Application.DTOs.StoreItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.StoreItemCommands.UpdateStoreItem
{
    public class UpdateStoreItemCommand : IRequest<OperationResult<string>>
    {
        public UpdateStoreItemDTO UpdatedStoreItem { get; set; }
        public int Id { get; set; }

        public UpdateStoreItemCommand(UpdateStoreItemDTO updatedStoreItem, int id)
        {
            UpdatedStoreItem = updatedStoreItem;
            Id = id;
        }
    }
}