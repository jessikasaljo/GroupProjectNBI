using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Application.DTOs.StoreDtos;
using Application.DTOs.Product;

namespace Application.Queries.StoreQueries.GetStoreById
{
    public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, OperationResult<StoreInventoryDTO>>
    {
        private readonly IGenericRepository<Store> storeRepository;
        private readonly IGenericRepository<Product> productRepository;
        private readonly IGenericRepository<ProductDetail> productDetailRepository;
        private readonly ILogger<GetStoreByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetStoreByIdQueryHandler(
            IGenericRepository<Store> _storeRepository,
            IGenericRepository<Product> _productRepository,
            IGenericRepository<ProductDetail> _productDetailRepository,
            ILogger<GetStoreByIdQueryHandler> _logger,
            IMemoryCache _memoryCache)
        {
            storeRepository = _storeRepository;
            productRepository = _productRepository;
            productDetailRepository = _productDetailRepository;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<StoreInventoryDTO>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var store = await storeRepository.GetByIdAsync(request.Id, cancellationToken);
                if (store == null)
                {
                    return OperationResult<StoreInventoryDTO>.FailureResult("Store not found", logger);
                }

                var inventory = new Dictionary<FullProductDTO, int>();

                foreach (var item in store.Inventory)
                {
                    var product = item;
                    var productDetail = await productDetailRepository.GetFirstOrDefaultAsync(
                        pd => pd.ProductId == product.Id, cancellationToken);

                    var fullProductDTO = new FullProductDTO(product, productDetail);
                    inventory.Add(fullProductDTO, 1); //ÄNDRA DETTA
                }

                var storeInventoryDTO = new StoreInventoryDTO
                {
                    Location = store.Location,
                    Inventory = inventory
                    //Add price here later
                };

                return OperationResult<StoreInventoryDTO>.SuccessResult(storeInventoryDTO, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<StoreInventoryDTO>.FailureResult($"Error occurred while fetching store inventory: {exception.Message}", logger);
            }
        }
    }
}
