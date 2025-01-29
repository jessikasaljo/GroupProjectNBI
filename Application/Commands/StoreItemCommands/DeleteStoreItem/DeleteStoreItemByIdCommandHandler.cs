using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StoreItemCommands.DeleteStoreItem
{
    public class DeleteStoreItemCommandHandler : IRequestHandler<DeleteStoreItemByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<StoreItem> storeItemRepository;
        private readonly ILogger logger;

        public DeleteStoreItemCommandHandler(IGenericRepository<StoreItem> _storeItemRepository, ILogger<DeleteStoreItemCommandHandler> _logger)
        {
            storeItemRepository = _storeItemRepository;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(DeleteStoreItemByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingStoreItem = await storeItemRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (existingStoreItem == null)
                {
                    return OperationResult<string>.FailureResult("StoreItem not found", logger);
                }

                await storeItemRepository.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("StoreItem deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting StoreItem: {exception.Message}", logger);
            }
        }
    }
}
