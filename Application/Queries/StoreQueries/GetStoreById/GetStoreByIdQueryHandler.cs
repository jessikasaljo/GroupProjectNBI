using Application.DTOs.Product;
using Application.DTOs.StoreDtos;
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
    public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, OperationResult<StoreInventoryDTO>>
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
                    var productDetails = await productDetailRepository.QueryAsync(
                          query => query
                              .Include(pd => pd.DetailInformation)
                              .Where(detail => detail.ProductId == item.Id),
                          cancellationToken);

                    var fullProductDTO = mapper.Map<FullProductDTO>(product);
                    fullProductDTO.DetailInformation = productDetails?.FirstOrDefault()?.DetailInformation.ToList() ?? new List<DetailInformation>();
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
