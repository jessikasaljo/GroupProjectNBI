using Application.Commands.CartCommands.AddCart;
using Application.DTOs.CartDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.CartCommands
{
    public class AddCartTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<AddCartCommandHandler>> _mockLogger;
        private readonly AddCartCommandHandler _handler;

        public AddCartTests()
        {
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<AddCartCommandHandler>>();
            _handler = new AddCartCommandHandler(_mockCartRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartAddedSuccessfully()
        {
            var command = new AddCartCommand(new AddCartDTO { UserId = 1 });

            _mockCartRepo.Setup(repo => repo.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Cart added successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartAlreadyExists()
        {
            var command = new AddCartCommand(new AddCartDTO { UserId = 1 });

            _mockCartRepo
                .Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart { UserId = 1 });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cart already exists for this user", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new AddCartCommand(new AddCartDTO { UserId = 2 });

            _mockCartRepo.Setup(repo => repo.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("An error occurred while processing the request", result.ErrorMessage);
        }
    }
}
