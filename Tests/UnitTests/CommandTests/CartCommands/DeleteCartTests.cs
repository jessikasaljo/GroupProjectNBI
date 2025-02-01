using Application.Commands.CartCommands.DeleteCart;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.CartCommands
{
    public class DeleteCartByIdTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<DeleteCartByIdCommandHandler>> _mockLogger;
        private readonly DeleteCartByIdCommandHandler _handler;

        public DeleteCartByIdTests()
        {
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<DeleteCartByIdCommandHandler>>();
            _handler = new DeleteCartByIdCommandHandler(_mockCartRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartNotFound()
        {
            var command = new DeleteCartByIdCommand(1);

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cart not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartDeletedSuccessfully()
        {
            var command = new DeleteCartByIdCommand(1);
            var existingCart = new Cart { Id = 1, UserId = 1 };

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCart);

            _mockCartRepo
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Cart deleted successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new DeleteCartByIdCommand(1);

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("An error occurred while processing the request", result.ErrorMessage);
        }
    }
}