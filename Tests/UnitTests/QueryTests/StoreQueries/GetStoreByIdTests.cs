using Application.DTOs.Product;
using Application.DTOs.StoreDtos;
using Application.DTOs.StoreItemDtos;
using Application.Queries.StoreQueries.GetStoreById;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.StoreQueries
{
    public class GetStoreByIdQueryHandlerTests
    {
        private readonly Mock<IGenericRepository<Store>> _mockStoreRepo;
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<IGenericRepository<ProductDetail>> _mockProductDetailRepo;
        private readonly Mock<ILogger<GetStoreByIdQueryHandler>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetStoreByIdQueryHandler _handler;

        public GetStoreByIdQueryHandlerTests()
        {
            _mockStoreRepo = new Mock<IGenericRepository<Store>>();
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockProductDetailRepo = new Mock<IGenericRepository<ProductDetail>>();
            _mockLogger = new Mock<ILogger<GetStoreByIdQueryHandler>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetStoreByIdQueryHandler(
                _mockStoreRepo.Object,
                _mockProductRepo.Object,
                _mockProductDetailRepo.Object,
                _mockLogger.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStoreNotFound()
        {
            var query = new GetStoreByIdQuery { Id = 1 };

            _mockStoreRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Store>, IQueryable<Store>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Store>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Store not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStoreFound()
        {
            var query = new GetStoreByIdQuery { Id = 1 };
            var product = new Product { Id = 1, Name = "Box of chocolates", Price = 100 };
            var store = new Store
            {
                Id = 1,
                Location = "New York",
                StoreItems = new List<StoreItem>
                {
                    new StoreItem { ProductId = 1, Quantity = 5, Product = new Product { Id = 1, Name = "Box of chocolates", Price = 100 } }
                }
            };

            var productDetails = new List<ProductDetail>
            {
                new ProductDetail { Product = product, DetailInformation = new List<DetailInformation> { new DetailInformation { Title = "Detail" } } }
            };

            var storeDto = new StoreWithInventoryDTO
            {
                Location = store.Location,
                Inventory = new List<FullStoreItemDTO>
                {
                    new FullStoreItemDTO
                    {
                        Product = new FullProductDTO { Name = "Box of chocolates", Price = 100 },
                        Quantity = 5
                    }
                }
            };

            _mockStoreRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Store>, IQueryable<Store>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Store> { store });

            _mockProductDetailRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<ProductDetail>, IQueryable<ProductDetail>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDetails);

            _mockMapper.Setup(mapper => mapper.Map<FullStoreItemDTO>(It.IsAny<StoreItem>()))
                .Returns(new FullStoreItemDTO { Quantity = 5, Product = new FullProductDTO { Name = "Box of chocolates", Price = 100 } });

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(storeDto.Location, result.Data!.Location);
            Assert.Single(result.Data.Inventory);
            Assert.Equal(100, result.Data.Inventory.First().Product.Price);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var query = new GetStoreByIdQuery { Id = 1 };

            _mockStoreRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Store>, IQueryable<Store>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while fetching store inventory: Database error", result.ErrorMessage);
        }
    }
}
