using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CartCommands.AddCart
{
    public class AddCartCommandHandler : IRequestHandler<AddCartCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Cart> _database;
        private readonly ILogger<AddCartCommandHandler> _logger;

        public AddCartCommandHandler(IGenericRepository<Cart> database, ILogger<AddCartCommandHandler> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(AddCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingCart = await _database.GetFirstOrDefaultAsync(c => c.UserId == request.NewCart.UserId, cancellationToken);
                if (existingCart != null)
                {
                    _logger.LogWarning("Cart already exists for UserId: {UserId}", request.NewCart.UserId);
                    return OperationResult<string>.FailureResult("Cart already exists for this user", _logger, 400);
                }

                var newCart = new Cart
                {
                    UserId = request.NewCart.UserId,
                    Items = new List<CartItem>()
                };

                await _database.AddAsync(newCart, cancellationToken);
                return OperationResult<string>.SuccessResult("Cart added successfully", _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding cart.");
                return OperationResult<string>.FailureResult("An error occurred while processing the request", _logger, 500);
            }
        }
    }
}
