using Application.DTOs.CartItemDtos;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CartItemQueries.GetCartItemById
{
    public class GetCartItemByIdQueryHandler : IRequestHandler<GetCartItemByIdQuery, OperationResult<CartItemDTO>>
    {
        private readonly IGenericRepository<CartItem> _repository;
        private readonly ILogger<GetCartItemByIdQueryHandler> _logger;
        private readonly IMemoryCache _memoryCache;

        public GetCartItemByIdQueryHandler(IGenericRepository<CartItem> repository, ILogger<GetCartItemByIdQueryHandler> logger, IMemoryCache memoryCache)
        {
            _repository = repository;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<OperationResult<CartItemDTO>> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"CartItem_{request.Id}";

            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out CartItemDTO? cachedCartItem))
                {
                    _logger.LogInformation("Cache hit. Retrieved CartItem with ID {Id} from cache.", request.Id);
                    return OperationResult<CartItemDTO>.SuccessResult(cachedCartItem!, _logger);
                }

                var cartItem = await _repository.QueryAsync(
                    query => query.Where(c => c.Id == request.Id).Include(c => c.Product),
                    cancellationToken);

                var cartItemEntity = cartItem?.FirstOrDefault();
                if (cartItemEntity == null)
                {
                    _logger.LogWarning("CartItem with ID {Id} not found.", request.Id);
                    return OperationResult<CartItemDTO>.FailureResult("CartItem not found", _logger, 404);
                }

                if (cartItemEntity.Product == null)
                {
                    _logger.LogWarning("Product associated with CartItem ID {CartItemId} not found.", request.Id);
                    return OperationResult<CartItemDTO>.FailureResult("Associated product not found", _logger, 404);
                }

                var cartItemDto = new CartItemDTO
                {
                    ProductId = cartItemEntity.ProductId,
                    ProductName = cartItemEntity.Product.Name ?? "Unknown Product",
                    ProductPrice = cartItemEntity.Product.Price,
                    Quantity = cartItemEntity.Quantity
                };

                _memoryCache.Set(cacheKey, cartItemDto, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Cache miss. CartItem with ID {Id} fetched from database and cached.", request.Id);

                return OperationResult<CartItemDTO>.SuccessResult(cartItemDto, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving CartItem with ID {Id}.", request.Id);
                return OperationResult<CartItemDTO>.FailureResult($"An error occurred: {ex.Message}", _logger, 500);
            }
        }
    }
}
