using Application.Queries.ProductQueries.GetAllProducts;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.ProductQueries
{
    public class GetAllProductsTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<ILogger<GetAllProductsQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockLogger = new Mock<ILogger<GetAllProductsQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            _handler = new GetAllProductsQueryHandler(
                _mockProductRepo.Object,
                _mockLogger.Object,
                _memoryCache
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoProductsFound()
        {
            var command = new GetAllProductsQuery { Page = 1, Hits = 10 };

            _mockProductRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No products found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductsFound()
        {
            var query = new GetAllProductsQuery { Page = 1, Hits = 10 };
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };

            _mockProductRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(products, result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFromRepository_WhenNoCacheHitOccurs()
        {
            var query = new GetAllProductsQuery { Page = 1, Hits = 10 };
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };

            _mockProductRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(products, result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetAllProductsQuery { Page = 1, Hits = 10 };

            _mockProductRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while getting products: Database error", result.ErrorMessage);
        }
    }
}