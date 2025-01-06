using Application.DTOs.Product;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, OperationResult<FullProductDTO?>>
    {
        private readonly IGenericRepository<Product> Database;
        private readonly IGenericRepository<ProductDetail> ProductDetailRepository;
        private readonly ILogger<GetProductByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetProductByIdQueryHandler(IGenericRepository<Product> _Database, IGenericRepository<ProductDetail> _DetailDatabase, ILogger<GetProductByIdQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            Database = _Database;
            ProductDetailRepository = _DetailDatabase;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<FullProductDTO?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Product_{request.Id}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out FullProductDTO? product))
                {

                    var productBase = await Database.GetByIdAsync(request.Id, cancellationToken);
                    var productDetail = await ProductDetailRepository.GetByIdAsync(request.Id, cancellationToken);
                    product = new FullProductDTO(productBase, productDetail);

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
                    return OperationResult<FullProductDTO?>.FailureResult("Product not found", logger);
                }

                return OperationResult<FullProductDTO?>.SuccessResult(product, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<FullProductDTO?>.FailureResult($"Error occurred while getting product: {exception.Message}", logger);
            }
        }
    }

}
