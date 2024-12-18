using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UpdateProduct
{
    public class UpdateProductByIdCommandHandler : IRequestHandler<UpdateProductByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> database;
        private readonly ILogger logger;
        public UpdateProductByIdCommandHandler(IGenericRepository<Product> _database, ILogger<UpdateProductByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateProductByIdCommand request, CancellationToken cancellationToken)
        {
            Product productToUpdate = request.UpdatedProduct;

            try
            {
                Product? existingProduct = null;
                existingProduct = await database.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (existingProduct == null)
                {
                    return OperationResult<string>.FailureResult("Product not found", logger);
                }
                await database.UpdateAsync(productToUpdate, cancellationToken);
                return OperationResult<string>.SuccessResult("Product updated successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking product: {exception.Message}", logger);
            }
        }
    }
}
