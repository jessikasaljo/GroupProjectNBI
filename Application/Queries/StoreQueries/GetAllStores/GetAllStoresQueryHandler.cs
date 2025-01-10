using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using Application.DTOs.StoreDtos;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.StoreQueries.GetAllStores
{
    public class GetAllStoresQueryHandler : IRequestHandler<GetAllStoresQuery, OperationResult<IEnumerable<StoreDto>>>
    {
        private readonly IGenericRepository<Store> database;
        private readonly ILogger<GetAllStoresQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetAllStoresQueryHandler(IGenericRepository<Store> _database, ILogger<GetAllStoresQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            database = _database;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<IEnumerable<StoreDto>>> Handle(GetAllStoresQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page;
            var size = request.Hits;

            var cacheKey = $"Stores_p{page}_s{size}";
            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Store>? stores))
                {
                    stores = await database.GetPageAsync(page, size, cancellationToken);
                    memoryCache.Set(cacheKey, stores, TimeSpan.FromMinutes(1));
                    logger.LogInformation($"Cache miss. Fetched stores for page:{page} with size:{size} from database and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }
                if (stores == null)
                {
                    return OperationResult<IEnumerable<StoreDto>>.FailureResult("No stores found", logger);
                }
                var storeDtos = stores.Select(store => new StoreDto { Location = store.Location }).ToList();

                return OperationResult<IEnumerable<StoreDto>>.SuccessResult(storeDtos, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<StoreDto>>.FailureResult($"Error occurred while getting stores: {exception.Message}", logger);
            }
        }
    }
}
