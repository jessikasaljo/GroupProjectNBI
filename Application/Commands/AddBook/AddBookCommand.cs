using Application.DTOs;
using Application.Helpers;
using MediatR;

namespace Application.Commands.AddBook
{
    public class AddBookCommand : IRequest<OperationResult<string>>
    {
        public NewBookDTO newBook { get; set; }
        public AddBookCommand(NewBookDTO bookToAdd)
        {
            newBook = bookToAdd;
        }
    }
}
