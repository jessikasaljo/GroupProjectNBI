using Application.Commands.CartCommands.UpdateCart;
using Application.DTOs.CartDtos;
using Application.DTOs.CartItemDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.CartCommands
{
    public class UpdateCartByIdTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<UpdateCartByIdCommandHandler>> _mockLogger;
        private readonly UpdateCartByIdCommandHandler _handler;

        public UpdateCartByIdTests()
        {
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<UpdateCartByIdCommandHandler>>();
            _handler = new UpdateCartByIdCommandHandler(_mockCartRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartNotFound()
        {
            var command = new UpdateCartByIdCommand(1, new CartDTO { UserId = 2, Items = new List<CartItemDTO>() });

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cart not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartUpdatedSuccessfully()
        {
            var command = new UpdateCartByIdCommand(1, new CartDTO
            {
                UserId = 2,
                Items = new List<CartItemDTO>
                {
                    new CartItemDTO { ProductId = 1, Quantity = 5 },
                    new CartItemDTO { ProductId = 2, Quantity = 3 }
                }
            });

            var existingCart = new Cart
            {
                Id = 1,
                UserId = 1,
                Items = new List<CartItem>
                {
                    new CartItem { Id = 1, ProductId = 1, Quantity = 1 },
                    new CartItem { Id = 2, ProductId = 2, Quantity = 1 }
                }
            };

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCart);

            _mockCartRepo
                .Setup(repo => repo.UpdateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Cart updated successfully", result.Data);
            Assert.Equal(2, existingCart.UserId);
            Assert.Equal(2, existingCart.Items.Count);

            Assert.Equal(5, existingCart.Items.First(i => i.ProductId == 1).Quantity);

            Assert.Equal(3, existingCart.Items.First(i => i.ProductId == 2).Quantity);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new UpdateCartByIdCommand(1, new CartDTO { UserId = 2, Items = new List<CartItemDTO>() });

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("An error occurred while processing the request", result.ErrorMessage);
        }
    }
}
