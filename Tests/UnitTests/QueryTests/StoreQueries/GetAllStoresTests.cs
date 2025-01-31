using Application.DTOs.StoreDtos;
using Application.Queries.StoreQueries.GetAllStores;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.StoreQueries
{
    public class GetAllStoresQueryHandlerTests
    {
        private readonly Mock<IGenericRepository<Store>> _mockStoreRepo;
        private readonly Mock<ILogger<GetAllStoresQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllStoresQueryHandler _handler;

        public GetAllStoresQueryHandlerTests()
        {
            _mockStoreRepo = new Mock<IGenericRepository<Store>>();
            _mockLogger = new Mock<ILogger<GetAllStoresQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllStoresQueryHandler(
                _mockStoreRepo.Object,
                _mockLogger.Object,
                _memoryCache,
                _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoStoresFound()
        {
            var query = new GetAllStoresQuery { Page = 1, Hits = 10 };

            _mockStoreRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Store>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("No stores found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStoresFound()
        {
            var query = new GetAllStoresQuery { Page = 1, Hits = 10 };
            var stores = new List<Store>
            {
                new Store { Id = 1, Location = "New York" },
                new Store { Id = 2, Location = "Los Angeles" }
            };
            var storeDtos = new List<StoreDto>
            {
                new StoreDto { Location = "New York" },
                new StoreDto { Location = "Los Angeles" }
            };

            _mockStoreRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(stores);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<StoreDto>>(It.IsAny<IEnumerable<Store>>()))
                .Returns(storeDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(storeDtos.Count, result.Data.Count());
            Assert.Equal("New York", result.Data.First().Location);
        }

        [Fact]
        public async Task Handle_ShouldReturnFromRepository_WhenNoCacheHitOccurs()
        {
            var query = new GetAllStoresQuery { Page = 1, Hits = 10 };
            var stores = new List<Store>
            {
                new Store { Id = 1, Location = "New York" },
                new Store { Id = 2, Location = "Los Angeles" }
            };
            var storeDtos = new List<StoreDto>
            {
                new StoreDto { Location = "New York" },
                new StoreDto { Location = "Los Angeles" }
            };

            _mockStoreRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(stores);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<StoreDto>>(It.IsAny<IEnumerable<Store>>()))
                .Returns(storeDtos);

            _memoryCache.Remove("Stores_p1_s10");

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(storeDtos.Count, result.Data.Count());
            Assert.Equal("New York", result.Data.First().Location);
            Assert.True(_memoryCache.TryGetValue("Stores_p1_s10", out _));
        }

        [Fact]
        public async Task Handle_ShouldReturnFromCache_WhenCacheHitOccurs()
        {
            var query = new GetAllStoresQuery { Page = 1, Hits = 10 };
            var stores = new List<Store>
            {
                new Store { Id = 1, Location = "New York" },
                new Store { Id = 2, Location = "Los Angeles" }
            };
            var storeDtos = new List<StoreDto>
            {
                new StoreDto { Location = "New York" },
                new StoreDto { Location = "Los Angeles" }
            };

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<StoreDto>>(It.IsAny<IEnumerable<Store>>()))
                .Returns(storeDtos);

            // Simulate a cache hit
            _memoryCache.Set("Stores_p1_s10", stores, TimeSpan.FromMinutes(1));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(storeDtos.Count, result.Data.Count());
            Assert.Equal("New York", result.Data.First().Location);
            _mockStoreRepo.Verify(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetAllStoresQuery { Page = 1, Hits = 10 };

            _mockStoreRepo.Setup(repo => repo.GetPageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while getting stores: Database error", result.ErrorMessage);
        }
    }
}
