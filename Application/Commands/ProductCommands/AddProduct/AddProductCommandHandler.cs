using Application.Commands.UserCommands.AddUser;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductCommands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> Database;
        private readonly ILogger<AddUserCommandHandler> logger;
        public AddProductCommandHandler(IGenericRepository<Product> _Database, ILogger<AddUserCommandHandler> _logger)
        {
            Database = _Database;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product
            {
                Name = request.newProduct.Name
            };

            Product? existingProduct = null;
            try
            {
                existingProduct = await Database.GetFirstOrDefaultAsync(a => a.Name == newProduct.Name, cancellationToken);
                if (existingProduct != null)
                {
                    return OperationResult<string>.FailureResult("Product already exists", logger);
                }
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking product: {exception.Message}", logger);
            }

            try
            {
                await Database.AddAsync(newProduct, cancellationToken);
                return OperationResult<string>.SuccessResult("Product added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding product: {exception.Message}", logger);
            }
        }

    }
}
