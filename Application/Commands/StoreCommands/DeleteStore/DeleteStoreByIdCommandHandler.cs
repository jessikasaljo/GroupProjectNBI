using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StoreCommands.DeleteStore
{
    public class DeleteStoreByIdCommandHandler : IRequestHandler<DeleteStoreByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Store> database;
        private readonly ILogger<DeleteStoreByIdCommandHandler> logger;

        public DeleteStoreByIdCommandHandler(IGenericRepository<Store> _database, ILogger<DeleteStoreByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(DeleteStoreByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingStore = await database.GetFirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
                if (existingStore == null)
                {
                    return OperationResult<string>.FailureResult("Store not found", logger);
                }

                await database.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("Store deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting store: {exception.Message}", logger);
            }
        }
    }
}
