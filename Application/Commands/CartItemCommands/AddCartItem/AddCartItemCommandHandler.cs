using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Commands.CartItemCommands.AddCartItem
{
    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly ILogger<AddCartItemCommandHandler> _logger;

        public AddCartItemCommandHandler(IGenericRepository<Product> productRepository, IGenericRepository<Cart> cartRepository, ILogger<AddCartItemCommandHandler> logger)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.NewItem == null)
                {
                    _logger.LogWarning("AddCartItemCommand received null item.");
                    return OperationResult<string>.FailureResult("Invalid cart item data", _logger, 400);
                }

                if (request.NewItem.Quantity <= 0)
                {
                    _logger.LogWarning("Invalid quantity: {Quantity}", request.NewItem.Quantity);
                    return OperationResult<string>.FailureResult("Quantity must be greater than zero", _logger, 400);
                }

                var product = await _productRepository.GetByIdAsync(request.NewItem.ProductId, cancellationToken);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", request.NewItem.ProductId);
                    return OperationResult<string>.FailureResult("Product not found", _logger, 404);
                }

                var cart = await _cartRepository.GetByIdAsync(request.NewItem.CartId, cancellationToken);
                if (cart == null)
                {
                    _logger.LogWarning("Cart with ID {CartId} not found.", request.NewItem.CartId);
                    return OperationResult<string>.FailureResult("Cart not found", _logger, 404);
                }

                var newItem = new CartItem
                {
                    ProductId = request.NewItem.ProductId,
                    Quantity = request.NewItem.Quantity,
                };

                cart.Items.Add(newItem);

                await _cartRepository.UpdateAsync(cart, cancellationToken);

                _logger.LogInformation("CartItem added successfully to CartId: {CartId} with ProductId: {ProductId}, Quantity: {Quantity}", request.NewItem.CartId, newItem.ProductId, newItem.Quantity);

                return OperationResult<string>.SuccessResult("CartItem added successfully", _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding CartItem.");
                return OperationResult<string>.FailureResult("An error occurred while processing the request", _logger, 500);
            }
        }
    }
}

