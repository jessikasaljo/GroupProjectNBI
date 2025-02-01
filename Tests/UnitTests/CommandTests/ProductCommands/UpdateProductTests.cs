using Application.Commands.ProductCommands.UpdateProduct;
using Application.DTOs.Product;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.ProductCommands
{
    public class UpdateProductByIdTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<ILogger<UpdateProductByIdCommandHandler>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProductByIdCommandHandler _handler;

        public UpdateProductByIdTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockLogger = new Mock<ILogger<UpdateProductByIdCommandHandler>>();
            _mockMapper = new Mock<IMapper>();

            _handler = new UpdateProductByIdCommandHandler(
                _mockProductRepo.Object,
                _mockLogger.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            var command = new UpdateProductByIdCommand(new ProductDTO { Name = "Updated Product", Price = 99.99m }, 1);

            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Product not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductIsUpdated()
        {
            var command = new UpdateProductByIdCommand(new ProductDTO { Name = "Updated Product", Price = 99.99m }, 1);
            var existingProduct = new Product { Id = 1, Name = "Old Product", Price = 50.00m };

            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);
            _mockProductRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Product updated successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new UpdateProductByIdCommand(new ProductDTO { Name = "Updated Product", Price = 99.99m }, 1);

            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while checking product: Database error", result.ErrorMessage);
        }
    }
}
