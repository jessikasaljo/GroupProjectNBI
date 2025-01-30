using Application.Commands.ProductCommands.DeleteProduct;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.ProjectCommands
{
    public class DeleteProductByIdTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<IGenericRepository<ProductDetail>> _mockDetailRepo;
        private readonly Mock<ILogger<DeleteProductByIdCommandHandler>> _mockLogger;
        private readonly DeleteProductByIdCommandHandler _handler;

        public DeleteProductByIdTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockDetailRepo = new Mock<IGenericRepository<ProductDetail>>();
            _mockLogger = new Mock<ILogger<DeleteProductByIdCommandHandler>>();

            _handler = new DeleteProductByIdCommandHandler(
                _mockProductRepo.Object,
                _mockDetailRepo.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            var command = new DeleteProductByIdCommand(1);

            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Product not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductIsDeleted()
        {
            var command = new DeleteProductByIdCommand(1);
            var existingProduct = new Product { Id = 1, Name = "Test Product" };

            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);
            _mockProductRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockDetailRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Product deleted successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new DeleteProductByIdCommand(1);

            _mockProductRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while deleting product: Database error", result.ErrorMessage);
        }
    }
}