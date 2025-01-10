using Application.Helpers;
using Domain.Models;
using MediatR;

namespace Application.Commands.StoreCommands.UpdateStore
{
    public class UpdateStoreByIdCommand : IRequest<OperationResult<string>>
    {
        public Store UpdatedStore { get; set; }
        public int Id { get; set; }
        public UpdateStoreByIdCommand(Store updatedStore, int id)
        {
            UpdatedStore = updatedStore;
            Id = id;
        }
    }
}
