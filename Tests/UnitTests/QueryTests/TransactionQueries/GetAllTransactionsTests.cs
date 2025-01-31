using Application.DTOs.TransactionDtos;
using Application.Queries.TransactionQuery;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.TransactionQuery
{
    public class GetAllTransactionsQueryHandlerTests
    {
        private readonly Mock<IGenericRepository<Transaction>> _mockTransactionRepo;
        private readonly Mock<ILogger<GetAllTransactionsQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly GetAllTransactionsQueryHandler _handler;

        public GetAllTransactionsQueryHandlerTests()
        {
            _mockTransactionRepo = new Mock<IGenericRepository<Transaction>>();
            _mockLogger = new Mock<ILogger<GetAllTransactionsQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            _handler = new GetAllTransactionsQueryHandler(
                _mockTransactionRepo.Object,
                _mockLogger.Object,
                _memoryCache
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoTransactionsFound()
        {
            var command = new GetAllTransactionsQuery { Page = 1, Hits = 10 };

            _mockTransactionRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Transaction>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No transactions found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTransactionsFound()
        {
            var query = new GetAllTransactionsQuery { Page = 1, Hits = 10 };
            var transactions = new List<Transaction>
            {
                new Transaction { StoreId = 1, TransactionDate = DateTime.UtcNow, Cart = new Cart { User = new User { UserName = "user1", UserPass = "1234", FirstName = "Jane", LastName = "Doe" } } },
                new Transaction { StoreId = 2, TransactionDate = DateTime.UtcNow, Cart = new Cart { User = new User { UserName = "user2", UserPass = "1234", FirstName = "John", LastName = "Doe" } } }
            };

            _mockTransactionRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactions);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(transactions.Select(t => new TransactionQueryDTO
            {
                UserName = t.Cart.User.UserName,
                storeId = t.StoreId,
                cartItems = t.Cart.Items,
                TransactionDate = t.TransactionDate
            }), result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFromRepository_WhenNoCacheHitOccurs()
        {
            var query = new GetAllTransactionsQuery { Page = 1, Hits = 10 };
            var transactions = new List<Transaction>
            {
                new Transaction { StoreId = 1, TransactionDate = DateTime.UtcNow, Cart = new Cart { User = new User { UserName = "user1", UserPass = "1234", FirstName = "Jane", LastName = "Doe" } } },
                new Transaction { StoreId = 2, TransactionDate = DateTime.UtcNow, Cart = new Cart { User = new User { UserName = "user2", UserPass = "1234", FirstName = "John", LastName = "Doe" } } }
            };

            _mockTransactionRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactions);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(transactions.Select(t => new TransactionQueryDTO
            {
                UserName = t.Cart.User.UserName,
                storeId = t.StoreId,
                cartItems = t.Cart.Items,
                TransactionDate = t.TransactionDate
            }), result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetAllTransactionsQuery { Page = 1, Hits = 10 };

            _mockTransactionRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while getting transactions: Database error", result.ErrorMessage);
        }
    }
}