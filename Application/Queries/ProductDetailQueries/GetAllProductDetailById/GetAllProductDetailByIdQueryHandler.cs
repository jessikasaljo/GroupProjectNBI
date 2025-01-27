using Application.Helpers;
using Application.Queries.ProductDetailQueries.GetProductDetailById;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.ProductDetailQueries.GetAllProductDetailById
{
    public class GetAllProductDetailByIdQueryHandler : IRequestHandler<GetAllProductDetailByIdQuery, OperationResult<IEnumerable<DetailInformation>>>
    {
        private readonly IGenericRepository<DetailInformation> detailDatabase;
        private readonly ILogger logger;
        private readonly IMemoryCache memoryCache;
        public GetAllProductDetailByIdQueryHandler(IGenericRepository<DetailInformation> _detailDatabase, ILogger<GetAllProductDetailByIdQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            detailDatabase = _detailDatabase;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<IEnumerable<DetailInformation>>> Handle(GetAllProductDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var productId = request.ProductId;
            var cacheKey = $"ProductDetail_p{productId}";
            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<DetailInformation>? productDetails))
                {
                    productDetails = await detailDatabase.QueryAsync(
                        query => query
                            .Where(detail => detail.ProductId == productId)
                        , cancellationToken);
                    memoryCache.Set(cacheKey, productDetails, TimeSpan.FromMinutes(1));
                    logger.LogInformation($"Cache miss. Fetched product details for product:{productId} from database and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }
                if (productDetails == null)
                {
                    return OperationResult<IEnumerable<DetailInformation>>.FailureResult("No product details found", logger);
                }
                return OperationResult<IEnumerable<DetailInformation>>.SuccessResult(productDetails, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<DetailInformation>>.FailureResult($"Error occurred while getting product details: {exception.Message}", logger);
            }
        }
    }
}
