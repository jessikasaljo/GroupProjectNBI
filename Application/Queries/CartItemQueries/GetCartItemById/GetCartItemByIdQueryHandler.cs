using Application.DTOs.CartItemDtos;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CartItemQueries.GetCartItemById
{
    public class GetCartItemByIdQueryHandler : IRequestHandler<GetCartItemByIdQuery, OperationResult<CartItemDTO>>
    {
        private readonly IGenericRepository<CartItem> _repository;
        private readonly ILogger<GetCartItemByIdQueryHandler> _logger;

        public GetCartItemByIdQueryHandler(IGenericRepository<CartItem> repository, ILogger<GetCartItemByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<CartItemDTO>> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            var cartItem = await _repository.GetFirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (cartItem == null)
            {
                return OperationResult<CartItemDTO>.FailureResult("CartItem not found", _logger);
            }

            var cartItemDto = new CartItemDTO
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                ProductPrice = cartItem.Product.Price,
                Quantity = cartItem.Quantity
            };

            return OperationResult<CartItemDTO>.SuccessResult(cartItemDto, _logger);
        }
    }
}
