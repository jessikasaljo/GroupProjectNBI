using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.UpdateAuthor
{
    public class UpdateAuthorByIdCommand : IRequest<OperationResult<string>>
    {
        public Author UpdatedAuthor { get; set; }
        public int Id { get; set; }
        public UpdateAuthorByIdCommand(Author updatedAuthor, int id)
        {
            UpdatedAuthor = updatedAuthor;
            Id = id;
        }
    }
}
