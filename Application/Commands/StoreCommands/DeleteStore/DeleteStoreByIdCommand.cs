using Application.Helpers;
using MediatR;

namespace Application.Commands.StoreCommands.DeleteStore
{
    public class DeleteStoreByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public DeleteStoreByIdCommand(int id)
        {
            Id = id;
        }
    }
}
