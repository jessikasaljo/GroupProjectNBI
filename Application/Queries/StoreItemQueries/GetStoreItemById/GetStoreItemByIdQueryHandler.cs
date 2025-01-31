using Application.DTOs.Product;
using Application.DTOs.StoreItemDtos;
using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.StoreItemQueries.GetStoreItemById
{
    public class GetStoreItemByIdQueryHandler : IRequestHandler<GetStoreItemByIdQuery, OperationResult<FullStoreItemDTO?>>
    {
        private readonly IGenericRepository<StoreItem> StoreItemDatabase;
        private readonly IGenericRepository<Store> StoreDatabase;
        private readonly ILogger<GetStoreItemByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper mapper;

        public GetStoreItemByIdQueryHandler(
            IGenericRepository<StoreItem> _StoreItemDatabase,
            IGenericRepository<Store> _StoreDatabase,
            ILogger<GetStoreItemByIdQueryHandler> _logger,
            IMemoryCache _memoryCache,
            IMapper _mapper)
        {
            StoreItemDatabase = _StoreItemDatabase;
            StoreDatabase = _StoreDatabase;
            logger = _logger;
            memoryCache = _memoryCache;
            mapper = _mapper;
        }

        public async Task<OperationResult<FullStoreItemDTO?>> Handle(
            GetStoreItemByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"StoreItem_{request.Id}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out FullStoreItemDTO? storeItem))
                {
                    var storeItemEntity = await StoreItemDatabase.QueryAsync(
                        query => query
                            .Include(si => si.Product)
                            .Where(si => si.Id == request.Id),
                        cancellationToken);

                    var storeItemData = storeItemEntity?.FirstOrDefault();

                    if (storeItemData != null)
                    {
                        var store = await StoreDatabase.QueryAsync(
                            query => query
                                .Include(s => s.StoreItems)
                                .Where(s => s.StoreItems.Any(si => si.Id == storeItemData.Id)),
                            cancellationToken);

                        var storeData = store?.FirstOrDefault();

                        storeItem = new FullStoreItemDTO
                        {
                            Product = mapper.Map<FullProductDTO>(storeItemData.Product),
                            Quantity = storeItemData.Quantity,
                            StoreLocation = storeData?.Location ?? string.Empty
                        };

                        memoryCache.Set(cacheKey, storeItem, TimeSpan.FromMinutes(5));

                        logger.LogInformation("Cache miss. Fetched storeItem with ID {Id} and cached at {Timestamp}", request.Id, DateTime.UtcNow);
                    }
                }
                else
                {
                    logger.LogInformation("Cache hit. Used cached storeItem with ID {Id} at {Timestamp}", request.Id, DateTime.UtcNow);
                }

                if (storeItem == null)
                {
                    return OperationResult<FullStoreItemDTO?>.FailureResult("StoreItem not found", logger);
                }

                return OperationResult<FullStoreItemDTO?>.SuccessResult(storeItem, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<FullStoreItemDTO?>.FailureResult($"Error occurred while getting storeItem: {exception.Message}", logger);
            }
        }
    }
}
