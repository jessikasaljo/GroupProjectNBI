using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UpdateBook
{
    public class UpdateBookByIdCommandHandler : IRequestHandler<UpdateBookByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Book> database;
        private readonly ILogger logger;

        public UpdateBookByIdCommandHandler(IGenericRepository<Book> _database, ILogger<UpdateBookByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateBookByIdCommand request, CancellationToken cancellationToken)
        {
            Book updatedBook = new()
            {
                Title = request.UpdatedBook.Title,
                Author = request.UpdatedBook.Author,
                Description = request.UpdatedBook.Description,
                Genre = request.UpdatedBook.Genre,
                Date = DateTime.Now
            };

            try
            {
                var existingBook = await database.GetFirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
                if (existingBook == null)
                {
                    return OperationResult<string>.FailureResult("Book not found", logger);
                }

                await database.UpdateAsync(updatedBook, cancellationToken);
                return OperationResult<string>.SuccessResult("Book updated successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while updating book: {exception.Message}", logger);
            }
        }
    }

}
