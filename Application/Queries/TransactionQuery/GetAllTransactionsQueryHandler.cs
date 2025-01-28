using Application.DTOs.TransactionDtos;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.TransactionQuery
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, OperationResult<IEnumerable<TransactionQueryDTO>>>
    {
        private readonly IGenericRepository<Transaction> transactionDatabase;
        private readonly ILogger<GetAllTransactionsQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetAllTransactionsQueryHandler(IGenericRepository<Transaction> _transactionDatabase, ILogger<GetAllTransactionsQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            transactionDatabase = _transactionDatabase;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<IEnumerable<TransactionQueryDTO>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page;
            var size = request.Hits;

            var cacheKey = $"Transactions_p{page}_s{size}";
            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<TransactionQueryDTO>? transactions))
                {
                    var allTransactions = await transactionDatabase.GetPageAsync(page, size, cancellationToken);
                    transactions = allTransactions.Select(t => new TransactionQueryDTO
                    {
                        UserName = t.Cart.User.UserName,
                        storeId = t.StoreId,
                        cartItems = t.Cart.Items,
                        TransactionDate = t.TransactionDate
                    });
                    memoryCache.Set(cacheKey, transactions, TimeSpan.FromMinutes(1));
                    logger.LogInformation($"Cache miss. Fetched transactions for page:{page} with size:{size} from database and cached at {DateTime.UtcNow}");
                }
                else
                {
                    logger.LogInformation($"Cache hit. Used cached {cacheKey} at {DateTime.UtcNow}");
                }
                if (transactions == null)
                {
                    return OperationResult<IEnumerable<TransactionQueryDTO>>.FailureResult("No transactions found", logger);
                }
                return OperationResult<IEnumerable<TransactionQueryDTO>>.SuccessResult(transactions, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<TransactionQueryDTO>>.FailureResult($"Error occurred while getting transactions: {exception.Message}", logger);
            }
        }
    }
}
