﻿using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CartItemCommands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<CartItem> _repository;
        private readonly ILogger<UpdateCartItemCommandHandler> _logger;

        public UpdateCartItemCommandHandler(IGenericRepository<CartItem> repository, ILogger<UpdateCartItemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var existingItem = await _repository.GetFirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (existingItem == null)
            {
                return OperationResult<string>.FailureResult("CartItem not found", _logger);
            }

            //existingItem.ProductId = request.UpdatedItem.ProductId;
            existingItem.Quantity = request.UpdatedItem.Quantity;

            await _repository.UpdateAsync(existingItem, cancellationToken);
            return OperationResult<string>.SuccessResult("CartItem updated successfully", _logger);
        }
    }
}
