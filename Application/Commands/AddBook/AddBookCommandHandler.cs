using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Book> Database;
        private readonly ILogger logger;
        public AddBookCommandHandler(IGenericRepository<Book> _Database, ILogger<AddBookCommandHandler> _logger)
        {
            Database = _Database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            Book bookToCreate = new()
            {
                Title = request.newBook.Title,
                Description = request.newBook.Description,
                Genre = request.newBook.Genre,
                Date = DateTime.Now
            };

            Book? existingBook = null;
            try
            {
                existingBook = await Database.GetFirstOrDefaultAsync(b => b.Title == bookToCreate.Title, cancellationToken);
                if (existingBook != null)
                {
                    return OperationResult<string>.FailureResult("Book already exists", logger);
                }
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking book: {exception.Message}", logger);
            }

            try
            {
                await Database.AddAsync(bookToCreate, cancellationToken);
                return OperationResult<string>.SuccessResult("Book added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding book: {exception.Message}", logger);
            }
        }

    }
}
