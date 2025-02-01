using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CartCommands.DeleteCart
{
    public class DeleteCartByIdCommandHandler : IRequestHandler<DeleteCartByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Cart> _database;
        private readonly ILogger<DeleteCartByIdCommandHandler> _logger;

        public DeleteCartByIdCommandHandler(IGenericRepository<Cart> database, ILogger<DeleteCartByIdCommandHandler> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(DeleteCartByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _database.GetFirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
                if (cart == null)
                {
                    return OperationResult<string>.FailureResult("Cart not found", _logger);
                }

                await _database.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("Cart deleted successfully", _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                return OperationResult<string>.FailureResult("An error occurred while processing the request", _logger);
            }
        }
    }
}
