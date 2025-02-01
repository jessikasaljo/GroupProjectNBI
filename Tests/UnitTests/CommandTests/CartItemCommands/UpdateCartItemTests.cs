
using Application.Commands.CartItemCommands.UpdateCartItem;
using Application.DTOs.CartItemDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.CartItemCommands
{
    public class UpdateCartItemTests
    {
        private readonly Mock<IGenericRepository<CartItem>> _mockCartItemRepo;
        private readonly Mock<ILogger<UpdateCartItemCommandHandler>> _mockLogger;
        private readonly UpdateCartItemCommandHandler _handler;

        public UpdateCartItemTests()
        {
            _mockCartItemRepo = new Mock<IGenericRepository<CartItem>>();
            _mockLogger = new Mock<ILogger<UpdateCartItemCommandHandler>>();
            _handler = new UpdateCartItemCommandHandler(_mockCartItemRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartItemUpdatedSuccessfully()
        {
            var command = new UpdateCartItemCommand(1, new UpdateCartItemDTO { Quantity = 3 });

            var existingCartItem = new CartItem { Id = 1, ProductId = 1, Quantity = 1 };

            _mockCartItemRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCartItem);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("CartItem updated successfully", result.Data);
            Assert.Equal(3, existingCartItem.Quantity);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartItemNotFound()
        {
            var command = new UpdateCartItemCommand(1, new UpdateCartItemDTO { Quantity = 3 });

            _mockCartItemRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartItem)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("CartItem not found", result.ErrorMessage);
        }

    }
}