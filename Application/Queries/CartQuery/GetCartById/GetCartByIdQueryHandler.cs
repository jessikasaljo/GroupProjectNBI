using Application.DTOs.CartDtos;
using Application.DTOs.CartItemDtos;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CartQuery.GetCartById
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, OperationResult<CartDTO>>
    {
        private readonly IGenericRepository<Cart> cartRepository;
        private readonly ILogger<GetCartByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetCartByIdQueryHandler(
            IGenericRepository<Cart> cartRepository,
            ILogger<GetCartByIdQueryHandler> logger,
            IMemoryCache memoryCache)
        {
            this.cartRepository = cartRepository;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        public async Task<OperationResult<CartDTO>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Cart_{request.Id}";

            try
            {
                // Check cache
                if (!memoryCache.TryGetValue(cacheKey, out CartDTO? cachedCart))
                {
                    logger.LogInformation("Cache miss. Fetching cart with ID {Id} from database.", request.Id);

                    // Fetch from database
                    var cart = await cartRepository.QueryAsync(
                        query => query
                            .Where(c => c.Id == request.Id)
                            .Include(c => c.Items)
                            .ThenInclude(ci => ci.Product),
                        cancellationToken);

                    var cartEntity = cart?.FirstOrDefault();

                    if (cartEntity == null)
                    {
                        var errorMessage = $"Cart with ID {request.Id} not found.";
                        logger.LogWarning(errorMessage);
                        return OperationResult<CartDTO>.FailureResult(errorMessage, logger, 404);
                    }

                    // Build DTO
                    var cartDto = new CartDTO
                    {
                        Id = cartEntity.Id,
                        UserId = cartEntity.UserId,
                        TotalPrice = cartEntity.Items.Sum(item => item.Product.Price * item.Quantity),
                        Items = cartEntity.Items.Select(item => new CartItemDTO
                        {
                            ProductId = item.ProductId,
                            ProductName = item.Product.Name,
                            ProductPrice = item.Product.Price,
                            Quantity = item.Quantity
                        }).ToList()
                    };

                    // Add to cache
                    memoryCache.Set(cacheKey, cartDto, TimeSpan.FromMinutes(5));
                    logger.LogInformation("Cart with ID {Id} cached.", request.Id);

                    return OperationResult<CartDTO>.SuccessResult(cartDto, logger);
                }

                // Cache hit
                logger.LogInformation("Cache hit. Cart with ID {Id} retrieved from cache.", request.Id);
                return OperationResult<CartDTO>.SuccessResult(cachedCart!, logger);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while fetching Cart with ID {request.Id}: {ex.Message}";
                logger.LogError(ex, errorMessage);
                return OperationResult<CartDTO>.FailureResult(errorMessage, logger, 500);
            }
        }
    }
}
