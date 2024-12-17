using Application.DTOs;
using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.AddAuthor
{
    public class AddAuthorCommand : IRequest<OperationResult<string>>
    {
        public NewAuthorDTO newAuthor { get; set; }
        public AddAuthorCommand(NewAuthorDTO authorToAdd)
        {
            newAuthor = authorToAdd;
        }
    }
}
