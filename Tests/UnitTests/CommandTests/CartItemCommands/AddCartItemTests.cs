
using Application.Commands.CartItemCommands.AddCartItem;
using Application.DTOs.CartItemDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.CommandTests.CartItemCommands
{
    public class AddCartItemTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<AddCartItemCommandHandler>> _mockLogger;
        private readonly AddCartItemCommandHandler _handler;

        public AddCartItemTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<AddCartItemCommandHandler>>();
            _handler = new AddCartItemCommandHandler(_mockProductRepo.Object, _mockCartRepo.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartItemAddedSuccessfully()
        {
            var command = new AddCartItemCommand(new AddCartItemDTO { CartId = 1, ProductId = 1, Quantity = 2 });

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product { Id = 1, Name = "Test Product" });

            _mockCartRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart { Id = 1, Items = new List<CartItem>() });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("CartItem added successfully", result.Data);
        }

       

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new AddCartItemCommand(new AddCartItemDTO { CartId = 1, ProductId = 1, Quantity = 2 });

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("An error occurred while processing the request", result.ErrorMessage);
        }
    }
}
