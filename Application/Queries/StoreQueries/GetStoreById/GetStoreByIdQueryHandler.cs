using Application.DTOs.Product;
using Application.DTOs.StoreDtos;
using Application.DTOs.StoreItemDtos;
using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.StoreQueries.GetStoreById
{
    public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, OperationResult<StoreWithInventoryDTO>>
    {
        private readonly IGenericRepository<Store> storeRepository;
        private readonly IGenericRepository<Product> productRepository;
        private readonly IGenericRepository<ProductDetail> productDetailRepository;
        private readonly ILogger<GetStoreByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper mapper;

        public GetStoreByIdQueryHandler(
            IGenericRepository<Store> _storeRepository,
            IGenericRepository<Product> _productRepository,
            IGenericRepository<ProductDetail> _productDetailRepository,
            ILogger<GetStoreByIdQueryHandler> _logger,
            IMemoryCache _memoryCache,
            IMapper _mapper)
        {
            storeRepository = _storeRepository;
            productRepository = _productRepository;
            productDetailRepository = _productDetailRepository;
            logger = _logger;
            memoryCache = _memoryCache;
            mapper = _mapper;
        }

        public async Task<OperationResult<StoreWithInventoryDTO>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var store = await storeRepository.QueryAsync(
                    query => query
                        .Where(s => s.Id == request.Id)
                        .Include(s => s.StoreItems)
                        .ThenInclude(si => si.Product),
                    cancellationToken);

                var storeEntity = store?.FirstOrDefault();
                if (storeEntity == null)
                {
                    return OperationResult<StoreWithInventoryDTO>.FailureResult("Store not found", logger);
                }

                logger.LogInformation($"Store has {storeEntity.StoreItems.Count} items.");

                var inventory = new List<FullStoreItemDTO>();

                foreach (var item in storeEntity.StoreItems)
                {
                    var product = item.Product;

                    var productDetails = await productDetailRepository.QueryAsync(
                        query => query
                            .Include(pd => pd.DetailInformation)
                            .Where(detail => detail.ProductId == item.ProductId),
                        cancellationToken);

                    var fullStoreItemDTO = mapper.Map<FullStoreItemDTO>(item);
                    fullStoreItemDTO.Product.DetailInformation = productDetails?.FirstOrDefault()?.DetailInformation.ToList() ?? new List<DetailInformation>();
                    fullStoreItemDTO.Quantity = item.Quantity;
                    fullStoreItemDTO.Product.Price = product.Price;

                    inventory.Add(fullStoreItemDTO);
                }

                var storeInventoryDTO = new StoreWithInventoryDTO
                {
                    Location = storeEntity.Location,
                    Inventory = inventory
                };

                return OperationResult<StoreWithInventoryDTO>.SuccessResult(storeInventoryDTO, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<StoreWithInventoryDTO>.FailureResult($"Error occurred while fetching store inventory: {exception.Message}", logger);
            }
        }
    }
}
