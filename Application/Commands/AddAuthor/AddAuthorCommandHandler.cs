using Application.Commands.AddUser;
using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Author> Database;
        private readonly ILogger<AddUserCommandHandler> logger;
        public AddAuthorCommandHandler(IGenericRepository<Author> _Database, ILogger<AddUserCommandHandler> _logger)
        {
            Database = _Database;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var newAuthor = new Author
            {
                Name = request.newAuthor.Name
            };

            Author? existingAuthor = null;
            try
            {
                existingAuthor = await Database.GetFirstOrDefaultAsync(a => a.Name == newAuthor.Name, cancellationToken);
                if (existingAuthor != null)
                {
                    return OperationResult<string>.FailureResult("Author already exists", logger);
                }
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking author: {exception.Message}", logger);
            }

            try
            {
                await Database.AddAsync(newAuthor, cancellationToken);
                return OperationResult<string>.SuccessResult("Author added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding author: {exception.Message}", logger);
            }
        }

    }
}
