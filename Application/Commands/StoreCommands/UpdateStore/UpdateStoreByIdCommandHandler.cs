using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StoreCommands.UpdateStore
{
    public class UpdateStoreByIdCommandHandler : IRequestHandler<UpdateStoreByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Store> database;
        private readonly ILogger<UpdateStoreByIdCommandHandler> logger;

        public UpdateStoreByIdCommandHandler(IGenericRepository<Store> _database, ILogger<UpdateStoreByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateStoreByIdCommand request, CancellationToken cancellationToken)
        {
            Store storeToUpdate = request.UpdatedStore;

            try
            {
                var existingStore = await database.GetFirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
                if (existingStore == null)
                {
                    return OperationResult<string>.FailureResult("Store not found", logger);
                }

                await database.UpdateAsync(storeToUpdate, cancellationToken);
                return OperationResult<string>.SuccessResult("Store updated successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while updating store: {exception.Message}", logger);
            }
        }
    }
}
