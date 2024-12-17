using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<IEnumerable<Book>>>
    {
        private readonly IGenericRepository<Book> Database;
        private readonly ILogger<GetAllBooksQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetAllBooksQueryHandler(IGenericRepository<Book> _database, ILogger<GetAllBooksQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            Database = _database;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<IEnumerable<Book>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "Books";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Book> books))
                {
                    books = await Database.GetAllAsync(cancellationToken);
                    memoryCache.Set(cacheKey, books, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                    logger.LogInformation("Cache miss. Fetched books from database and cached at {Timestamp}", DateTime.UtcNow);
                }
                else
                {
                    logger.LogInformation("Cache hit. Used cached Books at {Timestamp}", DateTime.UtcNow);
                }

                return OperationResult<IEnumerable<Book>>.SuccessResult(books, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<Book>>.FailureResult($"Error occurred while getting books: {exception.Message}", logger);
            }
        }
    }


}
