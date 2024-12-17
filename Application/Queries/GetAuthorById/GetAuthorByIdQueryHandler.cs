using Application.Helpers;
using Domain;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author?>>
    {
        private readonly IGenericRepository<Author> Database;
        private readonly ILogger<GetAuthorByIdQueryHandler> logger;
        private readonly IMemoryCache memoryCache;

        public GetAuthorByIdQueryHandler(IGenericRepository<Author> _Database, ILogger<GetAuthorByIdQueryHandler> _logger, IMemoryCache _memoryCache)
        {
            Database = _Database;
            logger = _logger;
            memoryCache = _memoryCache;
        }

        public async Task<OperationResult<Author?>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Author_{request.Id}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out Author? author))
                {
                    author = await Database.GetByIdAsync(request.Id, cancellationToken);

                    if (author != null)
                    {
                        memoryCache.Set(cacheKey, author, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                        logger.LogInformation("Cache miss. Fetched author with ID {Id} from database and cached at {Timestamp}", request.Id, DateTime.UtcNow);
                    }
                }
                else
                {
                    logger.LogInformation("Cache hit. Used cached author with ID {Id} at {Timestamp}", request.Id, DateTime.UtcNow);
                }

                if (author == null)
                {
                    return OperationResult<Author?>.FailureResult("Author not found", logger);
                }

                return OperationResult<Author?>.SuccessResult(author, logger);
            }
            catch (Exception exception)
            {
                return OperationResult<Author?>.FailureResult($"Error occurred while getting author: {exception.Message}", logger);
            }
        }
    }

}
