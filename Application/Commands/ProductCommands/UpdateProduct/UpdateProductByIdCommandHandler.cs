using Application.DTOs.Product;
using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductByIdCommandHandler : IRequestHandler<UpdateProductByIdCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Product> database;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        public UpdateProductByIdCommandHandler(IGenericRepository<Product> _database, ILogger<UpdateProductByIdCommandHandler> _logger, IMapper _mapper)
        {
            database = _database;
            logger = _logger;
            mapper = _mapper;
        }

        public async Task<OperationResult<string>> Handle(UpdateProductByIdCommand request, CancellationToken cancellationToken)
        {
            Product productToUpdate = mapper.Map<Product>(request.UpdatedProduct);

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
