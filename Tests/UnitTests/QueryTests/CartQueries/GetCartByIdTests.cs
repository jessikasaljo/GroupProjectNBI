using Application.DTOs.CartDtos;
using Application.DTOs.CartItemDtos;
using Application.Queries.CartQuery.GetCartById;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.CartQuery
{
    public class GetCartByIdTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<GetCartByIdQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly GetCartByIdQueryHandler _handler;

        public GetCartByIdTests()
        {
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<GetCartByIdQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _handler = new GetCartByIdQueryHandler(
                _mockCartRepo.Object,
                _mockLogger.Object,
                _memoryCache);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartNotFound()
        {
            var query = new GetCartByIdQuery(1);

            _mockCartRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Cart>, IQueryable<Cart>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Cart>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cart with ID 1 not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartFound()
        {
            var query = new GetCartByIdQuery(1);
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 2, Product = new Product { Price = 10, Name = "Product1" } },
                    new CartItem { ProductId = 2, Quantity = 1, Product = new Product { Price = 15, Name = "Product2" } }
                }
            };

            _mockCartRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Cart>, IQueryable<Cart>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Cart> { cart });

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(1, result.Data!.Id);
            Assert.Equal(35, result.Data.TotalPrice);
            Assert.Equal(2, result.Data.Items.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnFromCache_WhenCacheHitOccurs()
        {
            var query = new GetCartByIdQuery(1);
            var cartDto = new CartDTO
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 35,
                Items = new List<CartItemDTO>
                {
                    new CartItemDTO { ProductId = 1, ProductName = "Product1", ProductPrice = 10, Quantity = 2 },
                    new CartItemDTO { ProductId = 2, ProductName = "Product2", ProductPrice = 15, Quantity = 1 }
                }
            };

            _memoryCache.Set($"Cart_{query.Id}", cartDto, TimeSpan.FromMinutes(5));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(35, result.Data!.TotalPrice);
            Assert.Equal(2, result.Data.Items.Count());
            _mockCartRepo.Verify(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Cart>, IQueryable<Cart>>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetCartByIdQuery(1);

            _mockCartRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Cart>, IQueryable<Cart>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("An error occurred while fetching Cart with ID 1: Database error", result.ErrorMessage);
        }
    }
}