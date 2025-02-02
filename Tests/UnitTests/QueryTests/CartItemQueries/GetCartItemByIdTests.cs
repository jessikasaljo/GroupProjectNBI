
using Application.DTOs.CartItemDtos;
using Application.Queries.CartItemQueries.GetCartItemById;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.CartItemQueries
{
    public class GetCartItemByIdTests
    {
        private readonly Mock<IGenericRepository<CartItem>> _mockCartItemRepo;
        private readonly Mock<ILogger<GetCartItemByIdQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly GetCartItemByIdQueryHandler _handler;

        public GetCartItemByIdTests()
        {
            _mockCartItemRepo = new Mock<IGenericRepository<CartItem>>();
            _mockLogger = new Mock<ILogger<GetCartItemByIdQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _handler = new GetCartItemByIdQueryHandler(_mockCartItemRepo.Object, _mockLogger.Object, _memoryCache);
        }

        [Fact]
        public async Task Handle_ShouldReturnFromCache_WhenCacheHitOccurs()
        {
            var query = new GetCartItemByIdQuery(1);
            var cachedCartItem = new CartItemDTO { ProductId = 1, ProductName = "Product1", ProductPrice = 10, Quantity = 2 };

            _memoryCache.Set($"CartItem_{query.Id}", cachedCartItem, TimeSpan.FromMinutes(5));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Product1", result.Data!.ProductName);
            Assert.Equal(10, result.Data.ProductPrice);
            _mockCartItemRepo.Verify(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<CartItem>, IQueryable<CartItem>>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartItemNotFound()
        {
            var query = new GetCartItemByIdQuery(1);

            _mockCartItemRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<CartItem>, IQueryable<CartItem>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("CartItem not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartItemFound()
        {
            var query = new GetCartItemByIdQuery(1);
            var cartItem = new CartItem
            {
                Id = 1,
                ProductId = 1,
                Quantity = 2,
                Product = new Product { Name = "Product1", Price = 10 }
            };

            _mockCartItemRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<CartItem>, IQueryable<CartItem>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem> { cartItem });

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(1, result.Data!.ProductId);
            Assert.Equal("Product1", result.Data.ProductName);
            Assert.Equal(10, result.Data.ProductPrice);
            Assert.Equal(2, result.Data.Quantity);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetCartItemByIdQuery(1);

            _mockCartItemRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<CartItem>, IQueryable<CartItem>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("An error occurred", result.ErrorMessage);
        }
    }
}
