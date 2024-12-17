using Application.Commands.AddUser;
using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UpdateAuthor
{
    public class UpdateAuthorByIdCommandHandler : IRequestHandler<UpdateAuthorByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Author> database;
        private readonly ILogger logger;
        public UpdateAuthorByIdCommandHandler(IGenericRepository<Author> _database, ILogger<UpdateAuthorByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            Author authorToUpdate = new()
            {
                Name = request.UpdatedAuthor.Name,
                Books = request.UpdatedAuthor.Books
            };

            try
            {            
                Author? existingAuthor = null;
                existingAuthor = await database.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (existingAuthor == null)
                {
                    return OperationResult<string>.FailureResult("Author not found", logger);
                }
                await database.UpdateAsync(authorToUpdate, cancellationToken);
                return OperationResult<string>.SuccessResult("Auther updated successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking user: {exception.Message}", logger);
            }
        }
    }
}
