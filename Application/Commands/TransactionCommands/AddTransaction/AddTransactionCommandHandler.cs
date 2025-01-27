using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Models;

namespace Application.Commands.TransactionCommands.AddTransaction
{
    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Transaction> transactionDatabase;
        private readonly IGenericRepository<Cart> cartDatabase;
        private readonly ILogger<AddTransactionCommandHandler> logger;
        public AddTransactionCommandHandler(IGenericRepository<Transaction> _transactionDatabase, IGenericRepository<Cart> _cartDatabase, ILogger<AddTransactionCommandHandler> _logger)
        {
            transactionDatabase = _transactionDatabase;
            cartDatabase = _cartDatabase;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            var newTransaction = new Transaction
            {
                StoreId = request.newTransaction.StoreId,
                CartId = request.newTransaction.CartId,
                TransactionDate = request.newTransaction.TransactionDate,
            };
            try
            {
                var targetCart = await cartDatabase.GetByIdAsync(newTransaction.CartId, cancellationToken);
                if (targetCart == null)
                {
                    return OperationResult<string>.FailureResult("Cart not found", logger);
                }
                targetCart.Completed = true;
                await cartDatabase.UpdateAsync(targetCart, cancellationToken);
                await transactionDatabase.AddAsync(newTransaction, cancellationToken);
                return OperationResult<string>.SuccessResult("Transaction added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding transaction: {exception.Message}", logger);
            }
        }
    }
}
