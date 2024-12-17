using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.RemoveBook
{
    public class DeleteBookByIdCommandHandler : IRequestHandler<DeleteBookByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Book> database;
        private readonly ILogger logger;
        public DeleteBookByIdCommandHandler(IGenericRepository<Book> _database, ILogger<DeleteBookByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(DeleteBookByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingBook = await database.GetFirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
                if (existingBook == null)
                {
                    return OperationResult<string>.FailureResult("Book not found", logger);
                }

                await database.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("Book deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting book: {exception.Message}", logger);
            }
        }

    }
}
