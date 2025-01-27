using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductCommands.DeleteProduct
{
    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> productDatabase;
        private readonly IGenericRepository<ProductDetail> detailDatabase;
        private readonly ILogger logger;
        public DeleteProductByIdCommandHandler(IGenericRepository<Product> _productDatabase, IGenericRepository<ProductDetail> _detailDatabase, ILogger<DeleteProductByIdCommandHandler> _logger)
        {
            productDatabase = _productDatabase;
            detailDatabase = _detailDatabase;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await productDatabase.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (existingProduct == null)
                {
                    return OperationResult<string>.FailureResult("Product not found", logger);
                }

                await productDatabase.DeleteAsync(request.Id, cancellationToken);
                await detailDatabase.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("Product deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting product: {exception.Message}", logger);
            }
        }

    }
}
