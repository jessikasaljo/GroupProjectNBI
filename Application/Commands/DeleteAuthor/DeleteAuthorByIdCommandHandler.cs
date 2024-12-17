using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.DeleteAuthor
{
    public class DeleteAuthorByIdCommandHandler : IRequestHandler<DeleteAuthorByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Author> database;
        private readonly ILogger logger;
        public DeleteAuthorByIdCommandHandler(IGenericRepository<Author> _database, ILogger<DeleteAuthorByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(DeleteAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAuthor = await database.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (existingAuthor == null)
                {
                    return OperationResult<string>.FailureResult("Author not found", logger);
                }

                await database.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("Author deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting author: {exception.Message}", logger);
            }
        }

    }
}
