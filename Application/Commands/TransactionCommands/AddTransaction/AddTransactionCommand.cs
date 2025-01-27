using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.TransactionDtos;
using Application.Helpers;

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
