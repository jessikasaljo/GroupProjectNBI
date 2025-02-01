using Application.DTOs.TransactionDtos;
using Application.Helpers;
using MediatR;

namespace Application.Commands.TransactionCommands.AddTransaction
{
    public class AddTransactionCommand : IRequest<OperationResult<string>>
    {
        public TransactionDTO newTransaction { get; set; }
        public AddTransactionCommand(TransactionDTO transactionToAdd)
        {
            newTransaction = transactionToAdd;
        }
    }
}
