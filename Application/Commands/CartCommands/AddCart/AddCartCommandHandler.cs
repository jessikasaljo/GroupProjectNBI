using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Commands.CartCommands.AddCart
{
    public class AddCartCommandHandler : IRequestHandler<AddCartCommand, OperationResult<string>>
    {
        //private readonly IGenericRepository<Cart> _database;
        //private readonly ILogger<AddCartCommandHandler> _logger;

        //public AddCartCommandHandler(IGenericRepository<Cart> database, ILogger<AddCartCommandHandler> logger)
        //{
        //    _database = database;
        //    _logger = logger;
        //}

        //public async Task<OperationResult<string>> Handle(AddCartCommand request, CancellationToken cancellationToken)
        //{
        //    var newCart = new Cart
        //    {
        //        UserId = request.NewCart.UserId,
        //        Items = request.NewCart.Items.Select(item => new CartItem
        //        {
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity
        //        }).ToList()
        //    };

        //    await _database.AddAsync(newCart, cancellationToken);
        //    return OperationResult<string>.SuccessResult("Cart added successfully", _logger);
        //}
        private readonly IGenericRepository<Cart> _database;
        private readonly ILogger<AddCartCommandHandler> _logger;

        public AddCartCommandHandler(IGenericRepository<Cart> database, ILogger<AddCartCommandHandler> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(AddCartCommand request, CancellationToken cancellationToken)
        {
            var newCart = new Cart
            {
                UserId = request.NewCart.UserId,
                Items = new List<CartItem>()
            };

            await _database.AddAsync(newCart, cancellationToken);
            return OperationResult<string>.SuccessResult("Cart added successfully", _logger);
        }
    }
}
