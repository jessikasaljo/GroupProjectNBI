using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetAuthorById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, OperationResult<Product?>>
    {
        private readonly IGenericRepository<Product> Database;
        private readonly ILogger<GetProductByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetProductByIdQueryHandler(IGenericRepository<Product> _Database, ILogger<GetProductByIdQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            Database = _Database;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<Product?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Product_{request.Id}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out Product? product))
                {
                    product = await Database.GetByIdAsync(request.Id, cancellationToken);

                    if (product != null)
                    {
                        memoryCache.Set(cacheKey, product, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                        logger.LogInformation("Cache miss. Fetched product with ID {Id} from database and cached at {Timestamp}", request.Id, DateTime.UtcNow);
                    }
                }
                else
                {
                    logger.LogInformation("Cache hit. Used cached product with ID {Id} at {Timestamp}", request.Id, DateTime.UtcNow);
                }

                if (product == null)
                {
                    return OperationResult<Product?>.FailureResult("Product not found", logger);
                }

                return OperationResult<Product?>.SuccessResult(product, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<Product?>.FailureResult($"Error occurred while getting product: {exception.Message}", logger);
            }
        }
    }

}
