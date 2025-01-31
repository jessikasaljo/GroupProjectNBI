using Application.Helpers;
using MediatR;

namespace Application.Commands.StoreItemCommands.DeleteStoreItem
{
    public class DeleteStoreItemByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }

        public DeleteStoreItemByIdCommand(int id)
        {
            Id = id;
        }
    }
}