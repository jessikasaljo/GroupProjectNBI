using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using MediatR;
using Application.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<IEnumerable<Author>>>
    {
        private readonly IGenericRepository<Author> database;
        private readonly ILogger<GetAllAuthorsQueryHandler> logger;
        private readonly IMemoryCache memoryCache;
        public GetAllAuthorsQueryHandler(IGenericRepository<Author> _database, ILogger<GetAllAuthorsQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            database = _database;
            logger = _logger;
            memoryCache = _memoryCache;
        }
        public async Task<OperationResult<IEnumerable<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Authors";
            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Author> authors))
                {
                    authors = await database.GetAllAsync(cancellationToken);
                    memoryCache.Set(cacheKey, authors, TimeSpan.FromMinutes(5));
                    logger.LogInformation("Cache miss. Fetched Authors from database and cached at {Timestamp}", DateTime.UtcNow);
                }
                else
                {
                    logger.LogInformation("Cache hit. Used cached Authors at {Timestamp}", DateTime.UtcNow);
                }
                return OperationResult<IEnumerable<Author>>.SuccessResult(authors, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<IEnumerable<Author>>.FailureResult($"Error occurred while getting authors: {exception.Message}", logger);
            }
        }
    }
}
