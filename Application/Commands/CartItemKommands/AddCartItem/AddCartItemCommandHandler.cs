using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Commands.CartItemKommands.AddCartItem
{
    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<CartItem> _repository;
        private readonly ILogger<AddCartItemCommandHandler> _logger;

        public AddCartItemCommandHandler(IGenericRepository<CartItem> repository, ILogger<AddCartItemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var newItem = new CartItem
            {
                ProductId = request.NewItem.ProductId,
                Quantity = request.NewItem.Quantity
            };

            await _repository.AddAsync(newItem, cancellationToken);
            return OperationResult<string>.SuccessResult("CartItem added successfully", _logger);
        }
    }
}
