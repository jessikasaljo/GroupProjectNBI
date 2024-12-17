using Application.Helpers;
using MediatR;

namespace Application.Commands.RemoveBook
{
    public class DeleteBookByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public DeleteBookByIdCommand(int id)
        {
            Id = id;
        }
    }
}
