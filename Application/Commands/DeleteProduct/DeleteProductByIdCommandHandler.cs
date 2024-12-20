using Application.Commands.DeleteAuthor;
using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.DeleteProduct
{
    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> database;
        private readonly ILogger logger;
        public DeleteProductByIdCommandHandler(IGenericRepository<Product> _database, ILogger<DeleteProductByIdCommandHandler> _logger)
        {
            database = _database;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await database.GetFirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (existingProduct == null)
                {
                    return OperationResult<string>.FailureResult("Product not found", logger);
                }

                await database.DeleteAsync(request.Id, cancellationToken);
                return OperationResult<string>.SuccessResult("Product deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting product: {exception.Message}", logger);
            }
        }

    }
}
