using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StoreCommands.AddStore
{
    public class AddStoreCommandHandler : IRequestHandler<AddStoreCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Store> Database;
        private readonly ILogger<AddStoreCommandHandler> logger;

        public AddStoreCommandHandler(IGenericRepository<Store> _Database, ILogger<AddStoreCommandHandler> _logger)
        {
            Database = _Database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(AddStoreCommand request, CancellationToken cancellationToken)
        {
            var newStore = new Store
            {
                Location = request.newStore.Location,
            };

            Store? existingStore = null;
            try
            {
                existingStore = await Database.GetFirstOrDefaultAsync(s => s.Location == newStore.Location, cancellationToken);
                if (existingStore != null)
                {
                    return OperationResult<string>.FailureResult("Store already exists at this location", logger);
                }
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking store: {exception.Message}", logger);
            }

            try
            {
                await Database.AddAsync(newStore, cancellationToken);
                return OperationResult<string>.SuccessResult("Store added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding store: {exception.Message}", logger);
            }
        }
    }
}
