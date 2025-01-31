using Application.Helpers;
using Application.Queries.StoreItemQueries.GetAllStoreItems;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetAllStoreItemQueries.GetAllStoreItems
{
    public class GetAllStoreItemsQueryHandler : IRequestHandler<GetAllStoreItemsQuery, OperationResult<IEnumerable<StoreItem>>>
    {
        private readonly IGenericRepository<StoreItem> storeItemRepository;
        private readonly ILogger<GetAllStoreItemsQueryHandler> logger;
        private readonly IMemoryCache memoryCache;
        public GetAllStoreItemsQueryHandler(IGenericRepository<StoreItem> _database, ILogger<GetAllStoreItemsQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            storeItemRepository = _database;
            logger = _logger;
            memoryCache = _memoryCache;
        }
        public async Task<OperationResult<IEnumerable<StoreItem>>> Handle(GetAllStoreItemsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page;
            var size = request.Hits;

            var cacheKey = $"StoreItems_p{page}_s{size}";
            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<StoreItem>? storeItem))
                {
                    storeItem = await storeItemRepository.QueryAsync(
                        query => query
                            .Include(s => s.Product),
                            cancellationToken);


                    var storeItemEntity = storeItem?.FirstOrDefault();

                    memoryCache.Set(cacheKey, storeItem, TimeSpan.FromMinutes(1));
                    logger.LogInformation($"Cache miss. Fetched StoreItem for page:{page} with size:{size} from storeItemRepository and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }
                if (storeItem == null)
                {
                    return OperationResult<IEnumerable<StoreItem>>.FailureResult("No StoreItem found", logger);
                }
                return OperationResult<IEnumerable<StoreItem>>.SuccessResult(storeItem, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<StoreItem>>.FailureResult($"Error occurred while getting StoreItem: {exception.Message}", logger);
            }
        }
    }
}
