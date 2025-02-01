using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CartCommands.UpdateCart
{
    public class UpdateCartByIdCommandHandler : IRequestHandler<UpdateCartByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Cart> _database;
        private readonly ILogger<UpdateCartByIdCommandHandler> _logger;

        public UpdateCartByIdCommandHandler(IGenericRepository<Cart> database, ILogger<UpdateCartByIdCommandHandler> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateCartByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _database.GetFirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
                if (cart == null)
                {
                    return OperationResult<string>.FailureResult("Cart not found", _logger);
                }

                cart.UserId = request.UpdatedCart.UserId;
                cart.Items = request.UpdatedCart.Items.Select(item => new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList();

                await _database.UpdateAsync(cart, cancellationToken);
                return OperationResult<string>.SuccessResult("Cart updated successfully", _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return OperationResult<string>.FailureResult("An error occurred while processing the request", _logger);
            }
        }
    }
}