using Application.DTOs;
using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.UpdateBook
{
    public class UpdateBookByIdCommand : IRequest<OperationResult<string>>
    {
        public Book UpdatedBook { get; set; }
        public int Id { get; set; }
        public UpdateBookByIdCommand(Book updatedBook, int id)
        {
            UpdatedBook = updatedBook;
            Id = id;
        }
    }
}
