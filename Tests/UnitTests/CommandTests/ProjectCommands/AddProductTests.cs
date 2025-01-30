using Application.Commands.ProductCommands.AddProduct;
using Application.DTOs.Product;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.ProjectCommands
{
    public class AddProductTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<IGenericRepository<ProductDetail>> _mockDetailRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AddProductCommandHandler>> _mockLogger;
        private readonly AddProductCommandHandler _handler;

        public AddProductTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockDetailRepo = new Mock<IGenericRepository<ProductDetail>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AddProductCommandHandler>>();

            _handler = new AddProductCommandHandler(
                _mockProductRepo.Object,
                _mockDetailRepo.Object,
                _mockLogger.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductAlreadyExists()
        {
            var command = new AddProductCommand(new ProductDTO { Name = "Existing Product" });
            var existingProduct = new Product { Name = "Existing Product" };

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>()))
                .Returns(new Product { Name = "Existing Product" });
            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Product already exists", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductIsAdded()
        {
            var command = new AddProductCommand(new ProductDTO { Name = "New Product" });
            var newProduct = new Product { Name = "New Product" };

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDTO>())).Returns(newProduct);
            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);
            _mockProductRepo.Setup(repo => repo.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockDetailRepo.Setup(repo => repo.AddAsync(It.IsAny<ProductDetail>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Product added successfully", result.Data);
        }
    }
}