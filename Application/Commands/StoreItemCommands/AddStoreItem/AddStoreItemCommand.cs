using Application.DTOs.StoreItemDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.StoreItemCommands.AddStoreItem
{
    public class AddStoreItemCommand : IRequest<OperationResult<string>>
    {
        public AddStoreItemDTO NewStoreItem { get; set; }
        public AddStoreItemCommand(AddStoreItemDTO storeItemToAdd)
        {
            NewStoreItem = storeItemToAdd;
        }
    }
}
