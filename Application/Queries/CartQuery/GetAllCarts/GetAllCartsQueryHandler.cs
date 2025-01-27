using Application.DTOs.CartDtos;
using Application.DTOs.CartItemDtos;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CartQuery.GetAllCarts
{
    public class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, OperationResult<IEnumerable<CartDTO>>>
    {
        private readonly IGenericRepository<Cart> database;
        private readonly ILogger<GetAllCartsQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetAllCartsQueryHandler(IGenericRepository<Cart> _database, ILogger<GetAllCartsQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            database = _database;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<IEnumerable<CartDTO>>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page;
            var size = request.Hits;
            var cacheKey = $"Carts_p{page}_s{size}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Cart>? carts))
                {
                    carts = await database.GetPageAsync(page, size, cancellationToken);
                    memoryCache.Set(cacheKey, carts, TimeSpan.FromMinutes(1));
                    logger.LogInformation($"Cache miss. Fetched carts for page:{page} with size:{size} from database and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }

                var cartDtos = carts.Select(cart => new CartDTO
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    TotalPrice = cart.Items.Sum(item => item.Product.Price * item.Quantity),
                    Items = cart.Items.Select(item => new CartItemDTO
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        ProductPrice = item.Product.Price,
                        Quantity = item.Quantity
                    }).ToList()
                }).ToList();

                return OperationResult<IEnumerable<CartDTO>>.SuccessResult(cartDtos, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while fetching carts for page:{page} with size:{size}");
                return OperationResult<IEnumerable<CartDTO>>.FailureResult("An error occurred while retrieving carts.", logger);
            }
        }
    }
}
