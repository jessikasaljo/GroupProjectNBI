using Application.DTOs.Product;
using Application.Queries.ProductQueries.GetProductById;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitUnits.QueryTests.ProductQueries
{
    public class GetProductByIdTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<IGenericRepository<ProductDetail>> _mockDetailRepo;
        private readonly Mock<ILogger<GetProductByIdQueryHandler>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockDetailRepo = new Mock<IGenericRepository<ProductDetail>>();
            _mockLogger = new Mock<ILogger<GetProductByIdQueryHandler>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, FullProductDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetProductByIdQueryHandler(
                _mockProductRepo.Object,
                _mockDetailRepo.Object,
                _mockLogger.Object,
                _memoryCache,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductFound()
        {
            var productId = 1;
            var command = new GetProductByIdQuery(productId);
            var product = new Product { Id = productId, Name = "Product 1", Price = 100 };
            var productDetail = new ProductDetail
            {
                ProductId = productId,
                Product = product,
                DetailInformation = new List<DetailInformation> { new DetailInformation { Title = "Detail 1", Text = "Something interesting" } }
            };

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            _mockDetailRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<ProductDetail>, IQueryable<ProductDetail>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductDetail> { productDetail });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(productId, result.Data.Id);
            Assert.Single(result.Data.DetailInformation);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductNotFound()
        {
            var command = new GetProductByIdQuery(1);

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Product not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new GetProductByIdQuery(1);

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Contains("Error occurred while getting product: Database error", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFromCache_WhenProductFoundInCache()
        {
            var productId = 1;
            var command = new GetProductByIdQuery(productId);
            var product = new Product { Id = productId, Name = "Product 1", Price = 100 };
            var productDetail = new ProductDetail
            {
                ProductId = productId,
                Product = product,
                DetailInformation = new List<DetailInformation> { new DetailInformation { Title = "Detail 1", Text = "Something interesting" } }
            };

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            _mockDetailRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<ProductDetail>, IQueryable<ProductDetail>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductDetail> { productDetail });

            await _handler.Handle(command, CancellationToken.None);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(productId, result.Data.Id);
            Assert.Single(result.Data.DetailInformation);
        }
    }
}