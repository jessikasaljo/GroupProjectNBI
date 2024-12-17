using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<Book?>>
    {
        private readonly IGenericRepository<Book> Database;
        private readonly ILogger<GetBookByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetBookByIdQueryHandler(IGenericRepository<Book> _Database, ILogger<GetBookByIdQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            Database = _Database;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<Book?>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Book_{request.Id}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out Book? book))
                {
                    book = await Database.GetByIdAsync(request.Id, cancellationToken);

                    if (book != null)
                    {
                        memoryCache.Set(cacheKey, book, TimeSpan.FromMinutes(5));
                        logger.LogInformation("Cache miss. Fetched book with ID {Id} from database and cached at {Timestamp}", request.Id, DateTime.UtcNow);
                    }
                }
                else
                {
                    logger.LogInformation("Cache hit. Used cached book with ID {Id} at {Timestamp}", request.Id, DateTime.UtcNow);
                }

                if (book == null)
                {
                    return OperationResult<Book?>.FailureResult("Book not found", logger);
                }

                return OperationResult<Book?>.SuccessResult(book, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<Book?>.FailureResult($"Error occurred while getting book: {exception.Message}", logger);
            }
        }
    }


}
