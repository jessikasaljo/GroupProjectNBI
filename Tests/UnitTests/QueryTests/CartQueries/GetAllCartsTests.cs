using Application.Queries.CartQuery.GetAllCarts;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.CartQuery
{
    public class GetAllCartsTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<GetAllCartsQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly GetAllCartsQueryHandler _handler;

        public GetAllCartsTests()
        {
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<GetAllCartsQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _handler = new GetAllCartsQueryHandler(
                _mockCartRepo.Object,
                _mockLogger.Object,
                _memoryCache);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoCartsFound()
        {
            var query = new GetAllCartsQuery { Page = 1, Hits = 10 };

            _mockCartRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Cart>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No carts found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartsFound()
        {
            var query = new GetAllCartsQuery { Page = 1, Hits = 10 };
            var carts = new List<Cart>
            {
                new Cart { Id = 1, UserId = 1, Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, Product = new Product { Price = 10, Name = "Product1" } } } },
                new Cart { Id = 2, UserId = 2, Items = new List<CartItem> { new CartItem { ProductId = 2, Quantity = 1, Product = new Product { Price = 15, Name = "Product2" } } } }
            };

            _mockCartRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(carts);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(carts.Count, result.Data!.Count());
            Assert.Equal(20, result.Data!.First().TotalPrice);
        }

        [Fact]
        public async Task Handle_ShouldReturnFromRepository_WhenNoCacheHitOccurs()
        {
            var query = new GetAllCartsQuery { Page = 1, Hits = 10 };
            var carts = new List<Cart>
            {
                new Cart { Id = 1, UserId = 1, Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, Product = new Product { Price = 10, Name = "Product1" } } } },
                new Cart { Id = 2, UserId = 2, Items = new List<CartItem> { new CartItem { ProductId = 2, Quantity = 1, Product = new Product { Price = 15, Name = "Product2" } } } }
            };

            _mockCartRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(carts);

            _memoryCache.Remove("Carts_p1_s10");

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(carts.Count, result.Data!.Count());
            Assert.Equal(20, result.Data!.First().TotalPrice);
            Assert.True(_memoryCache.TryGetValue("Carts_p1_s10", out _));
        }

        [Fact]
        public async Task Handle_ShouldReturnFromCache_WhenCacheHitOccurs()
        {
            var query = new GetAllCartsQuery { Page = 1, Hits = 10 };
            var carts = new List<Cart>
            {
                new Cart { Id = 1, UserId = 1, Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, Product = new Product { Price = 10, Name = "Product1" } } } },
                new Cart { Id = 2, UserId = 2, Items = new List<CartItem> { new CartItem { ProductId = 2, Quantity = 1, Product = new Product { Price = 15, Name = "Product2" } } } }
            };

            _memoryCache.Set("Carts_p1_s10", carts, TimeSpan.FromMinutes(1));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(carts.Count, result.Data!.Count());
            Assert.Equal(20, result.Data!.First().TotalPrice);
            _mockCartRepo.Verify(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetAllCartsQuery { Page = 1, Hits = 10 };

            _mockCartRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("An error occurred while retrieving carts.", result.ErrorMessage);
        }
    }
}
