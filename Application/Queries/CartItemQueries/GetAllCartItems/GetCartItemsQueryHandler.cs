using Application.DTOs.CartItemDtos;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CartItemQueries.GetAllCartItems
{
    public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, OperationResult<IEnumerable<CartItemDTO>>>
    {
        private readonly IGenericRepository<Cart> _repository;
        private readonly ILogger<GetCartItemsQueryHandler> _logger;

        public GetCartItemsQueryHandler(IGenericRepository<Cart> repository, ILogger<GetCartItemsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<CartItemDTO>>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var cart = await _repository.GetFirstOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);
            if (cart == null)
            {
                return OperationResult<IEnumerable<CartItemDTO>>.FailureResult("Cart not found", _logger);
            }

            var cartItems = cart.Items.Select(item => new CartItemDTO
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                ProductPrice = item.Product.Price,
                Quantity = item.Quantity
            }).ToList();

            return OperationResult<IEnumerable<CartItemDTO>>.SuccessResult(cartItems, _logger);
        }
    }
}
