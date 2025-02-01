using Application.Commands.TransactionCommands.AddTransaction;
using Application.DTOs.TransactionDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.CommandTests.TransactionCommands
{
    public class AddTransactionTests
    {
        private readonly Mock<IGenericRepository<Transaction>> _mockTransactionRepo;
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<AddTransactionCommandHandler>> _mockLogger;
        private readonly AddTransactionCommandHandler _handler;

        public AddTransactionTests()
        {
            _mockTransactionRepo = new Mock<IGenericRepository<Transaction>>();
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<AddTransactionCommandHandler>>();

            _handler = new AddTransactionCommandHandler(
                _mockTransactionRepo.Object,
                _mockCartRepo.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartNotFound()
        {
            var command = new AddTransactionCommand(new TransactionDTO { StoreId = 1, CartId = 1, TransactionDate = DateTime.UtcNow });

            _mockCartRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cart not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCartAlreadyCompleted()
        {
            var command = new AddTransactionCommand(new TransactionDTO { StoreId = 1, CartId = 1, TransactionDate = DateTime.UtcNow });
            var existingCart = new Cart { Id = 1, Completed = true };

            _mockCartRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCart);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Cart already completed", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTransactionIsAdded()
        {
            var command = new AddTransactionCommand(new TransactionDTO { StoreId = 1, CartId = 1, TransactionDate = DateTime.UtcNow });
            var existingCart = new Cart { Id = 1, Completed = false };

            _mockCartRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCart);
            _mockTransactionRepo.Setup(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Transaction added successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new AddTransactionCommand(new TransactionDTO { StoreId = 1, CartId = 1, TransactionDate = DateTime.UtcNow });

            _mockCartRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while adding transaction: Database error", result.ErrorMessage);
        }
    }
}