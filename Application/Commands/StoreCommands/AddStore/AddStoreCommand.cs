using Application.DTOs.StoreDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.StoreCommands.AddStore
{
    public class AddStoreCommand : IRequest<OperationResult<string>>
    {
        public AddStoreDTO newStore { get; set; }
        public AddStoreCommand(AddStoreDTO storeToAdd)
        {
            newStore = storeToAdd;
        }
    }

}
