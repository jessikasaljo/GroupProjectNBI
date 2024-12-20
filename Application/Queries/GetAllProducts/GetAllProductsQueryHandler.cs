using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetAllAuthors
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, OperationResult<IEnumerable<Product>>>
    {
        private readonly IGenericRepository<Product> database;
        private readonly ILogger<GetAllProductsQueryHandler> logger;
        private readonly IMemoryCache memoryCache;
        public GetAllProductsQueryHandler(IGenericRepository<Product> _database, ILogger<GetAllProductsQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            database = _database;
            logger = _logger;
            memoryCache = _memoryCache;
        }
        public async Task<OperationResult<IEnumerable<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var page = request.PageNumber;
            var size = request.PageSize;

            var cacheKey = $"Products_p{page}_s{size}";
            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Product>? products))
                {
                    products = await database.GetPageAsync(page, size, cancellationToken);
                    memoryCache.Set(cacheKey, products, TimeSpan.FromMinutes(5));
                    logger.LogInformation($"Cache miss. Fetched products for page:{page} with size:{size} from database and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }
                return OperationResult<IEnumerable<Product>>.SuccessResult(products, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<Product>>.FailureResult($"Error occurred while getting products: {exception.Message}", logger);
            }
        }
    }
}
