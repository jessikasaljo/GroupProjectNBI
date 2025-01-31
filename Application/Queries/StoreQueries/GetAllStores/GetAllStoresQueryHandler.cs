using Application.DTOs.StoreDtos;
using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
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
        private readonly IMapper mapper;

        public GetAllStoresQueryHandler(IGenericRepository<Store> _database, ILogger<GetAllStoresQueryHandler> _logger, IMemoryCache _memoryCache, IMapper _mapper)
        {
            database = _database;
            logger = _logger;
            memoryCache = _memoryCache;
            mapper = _mapper;
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
                    logger.LogInformation($"Cache miss. Fetched stores for page:{page} with size:{size} from storeItemRepository and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }

                if (stores == null || !stores.Any())
                {
                    return OperationResult<IEnumerable<StoreDto>>.FailureResult("No stores found", logger);
                }
                var storeDtos = mapper.Map<IEnumerable<StoreDto>>(stores);

                return OperationResult<IEnumerable<StoreDto>>.SuccessResult(storeDtos, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<StoreDto>>.FailureResult($"Error occurred while getting stores: {exception.Message}", logger);
            }
        }
    }
}
