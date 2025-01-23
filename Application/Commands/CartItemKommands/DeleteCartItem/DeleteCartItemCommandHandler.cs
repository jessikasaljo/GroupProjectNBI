using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CartItemKommands.DeleteCartItem
{
    public class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<CartItem> _repository;
        private readonly ILogger<DeleteCartItemCommandHandler> _logger;

        public DeleteCartItemCommandHandler(IGenericRepository<CartItem> repository, ILogger<DeleteCartItemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var existingItem = await _repository.GetFirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (existingItem == null)
            {
                return OperationResult<string>.FailureResult("CartItem not found", _logger);
            }

            await _repository.DeleteAsync(request.Id, cancellationToken);
            return OperationResult<string>.SuccessResult("CartItem deleted successfully", _logger);
        }
    }
}
