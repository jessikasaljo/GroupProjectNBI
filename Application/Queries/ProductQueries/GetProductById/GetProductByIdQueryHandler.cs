using Application.DTOs.Product;
using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.ProductQueries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, OperationResult<FullProductDTO?>>
    {
        private readonly IGenericRepository<Product> ProductDatabase;
        private readonly IGenericRepository<ProductDetail> DetailDatabase;
        private readonly ILogger<GetProductByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(IGenericRepository<Product> _ProductDatabase, IGenericRepository<ProductDetail> _DetailDatabase, ILogger<GetProductByIdQueryHandler> _logger, IMemoryCache _memoryCache, IMapper _mapper)
        {
            ProductDatabase = _ProductDatabase;
            DetailDatabase = _DetailDatabase;
            logger = _logger;
            memoryCache = _memoryCache;
            mapper = _mapper;
        }

        public async Task<OperationResult<FullProductDTO?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Product_{request.Id}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out FullProductDTO? product))
                {
                    var productBase = await ProductDatabase.GetByIdAsync(request.Id, cancellationToken);
                    if (productBase == null)
                    {
                        return OperationResult<FullProductDTO?>.FailureResult("Product not found", logger);
                    }

                    var productDetails = await DetailDatabase.QueryAsync(
                        query => query
                            .Include(pd => pd.DetailInformation)
                            .Where(detail => detail.ProductId == request.Id),
                        cancellationToken);

                    var firstProductDetail = productDetails?.FirstOrDefault();

                    product = mapper.Map<FullProductDTO>(productBase);
                    product.DetailInformation = firstProductDetail?.DetailInformation.ToList() ?? new List<DetailInformation>();

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
