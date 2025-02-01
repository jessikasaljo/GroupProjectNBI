using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StoreItemCommands.UpdateStoreItem
{
    public class UpdateStoreItemCommandHandler : IRequestHandler<UpdateStoreItemCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<StoreItem> storeItemRepository;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public UpdateStoreItemCommandHandler(
            IGenericRepository<StoreItem> _storeItemRepository,
            ILogger<UpdateStoreItemCommandHandler> _logger,
            IMapper _mapper)
        {
            storeItemRepository = _storeItemRepository;
            logger = _logger;
            mapper = _mapper;
        }

        public async Task<OperationResult<string>> Handle(UpdateStoreItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingStoreItem = await storeItemRepository.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (existingStoreItem == null)
                {
                    return OperationResult<string>.FailureResult("StoreItem not found", logger);
                }

                existingStoreItem.Quantity = request.UpdatedStoreItem.Quantity;

                await storeItemRepository.UpdateAsync(existingStoreItem, cancellationToken);
                return OperationResult<string>.SuccessResult("StoreItem updated successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while updating StoreItem: {exception.Message}", logger);
            }
        }
    }
}
