
using Application.Commands.CartItemCommands.DeleteCartItem;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.CartItemCommands
{
    public class DeleteCartItemTests
    {
        private readonly Mock<IGenericRepository<CartItem>> _mockCartItemRepo;
        private readonly Mock<ILogger<DeleteCartItemCommandHandler>> _mockLogger;
        private readonly DeleteCartItemCommandHandler _handler;

        public DeleteCartItemTests()
        {
            _mockCartItemRepo = new Mock<IGenericRepository<CartItem>>();
            _mockLogger = new Mock<ILogger<DeleteCartItemCommandHandler>>();
            _handler = new DeleteCartItemCommandHandler(_mockCartItemRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartItemNotFound()
        {
            var command = new DeleteCartItemCommand(1);

            _mockCartItemRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartItem)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("CartItem not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartItemDeletedSuccessfully()
        {
            var command = new DeleteCartItemCommand(1);
            var existingCartItem = new CartItem { Id = 1, ProductId = 1, Quantity = 2 };

            _mockCartItemRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCartItem);

            _mockCartItemRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("CartItem deleted successfully", result.Data);
        }

    }
}
