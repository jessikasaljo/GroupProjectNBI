using Application.Helpers;
using MediatR;

namespace Application.Commands.DeleteAuthor
{
    public class DeleteAuthorByIdCommand : IRequest<OperationResult<string>>
    {
        public int Id { get; set; }
        public DeleteAuthorByIdCommand(int id)
        {
            Id = id;
        }
    }
}
