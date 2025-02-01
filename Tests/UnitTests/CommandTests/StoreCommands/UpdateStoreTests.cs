using Application.Commands.StoreCommands.UpdateStore;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.StoreCommands
{
    public class UpdateStoreByIdCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<Store>> _mockStoreRepo;
        private readonly Mock<ILogger<UpdateStoreByIdCommandHandler>> _mockLogger;
        private readonly UpdateStoreByIdCommandHandler _handler;

        public UpdateStoreByIdCommandHandlerTests()
        {
            _mockStoreRepo = new Mock<IGenericRepository<Store>>();
            _mockLogger = new Mock<ILogger<UpdateStoreByIdCommandHandler>>();
            _handler = new UpdateStoreByIdCommandHandler(_mockStoreRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStoreNotFound()
        {
            var updatedStore = new Store { Id = 1, Location = "New York" };
            var command = new UpdateStoreByIdCommand(updatedStore, 1);

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Store)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Store not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStoreUpdated()
        {
            var updatedStore = new Store { Id = 1, Location = "New York" };
            var command = new UpdateStoreByIdCommand(updatedStore, 1);
            var existingStore = new Store { Id = 1, Location = "Miami" };

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingStore);

            _mockStoreRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Store>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Store updated successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var updatedStore = new Store { Id = 1, Location = "New York" };
            var command = new UpdateStoreByIdCommand(updatedStore, 1);

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while updating store: Database error", result.ErrorMessage);
        }
    }
}
