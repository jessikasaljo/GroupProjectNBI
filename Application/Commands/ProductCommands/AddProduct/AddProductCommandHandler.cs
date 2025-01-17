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
        private readonly IGenericRepository<Product> productDatabase;
        private readonly IGenericRepository<ProductDetail> detailDatabase;
        private readonly ILogger<AddProductCommandHandler> logger;
        public AddProductCommandHandler(IGenericRepository<Product> _productDatabase, IGenericRepository<ProductDetail> _detailDatabase, ILogger<AddProductCommandHandler> _logger)
        {
            productDatabase = _productDatabase;
            detailDatabase = _detailDatabase;
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
                existingProduct = await productDatabase.GetFirstOrDefaultAsync(a => a.Name == newProduct.Name, cancellationToken);
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
                await productDatabase.AddAsync(newProduct, cancellationToken);
                await detailDatabase.AddAsync(new ProductDetail { Product = newProduct }, cancellationToken);
                return OperationResult<string>.SuccessResult("Product added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding product: {exception.Message}", logger);
            }
        }

    }
}
