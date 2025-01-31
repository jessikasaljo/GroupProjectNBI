using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StoreItemCommands.AddStoreItem
{
    public class AddStoreItemCommandHandler : IRequestHandler<AddStoreItemCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Store> _storeRepository;
        private readonly ILogger<AddStoreItemCommandHandler> _logger;

        public AddStoreItemCommandHandler(
            IGenericRepository<Product> productRepository,
            IGenericRepository<Store> storeRepository,
            ILogger<AddStoreItemCommandHandler> logger)
        {
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(AddStoreItemCommand request, CancellationToken cancellationToken)
        {
            Product? existingProduct = await _productRepository.GetByIdAsync(request.NewStoreItem.ProductId, cancellationToken);
            if (existingProduct == null)
            {
                return OperationResult<string>.FailureResult($"Product with ID {request.NewStoreItem.ProductId} does not exist.", _logger);
            }

            Store? store = await _storeRepository.GetByIdAsync(request.NewStoreItem.StoreId, cancellationToken);
            if (store == null)
            {
                return OperationResult<string>.FailureResult($"Store with ID {request.NewStoreItem.StoreId} does not exist.", _logger);
            }

            var existingStoreItem = store.StoreItems.FirstOrDefault(si => si.ProductId == request.NewStoreItem.ProductId);
            if (existingStoreItem != null)
            {
                return OperationResult<string>.FailureResult("StoreItem for this product already exists in the selected store.", _logger);
            }

            var newStoreItem = new StoreItem
            {
                ProductId = request.NewStoreItem.ProductId,
                Quantity = request.NewStoreItem.Quantity,
                Product = existingProduct
            };

            store.StoreItems.Add(newStoreItem);

            await _storeRepository.UpdateAsync(store, cancellationToken);

            return OperationResult<string>.SuccessResult("StoreItem added successfully.", _logger);
        }
    }
}