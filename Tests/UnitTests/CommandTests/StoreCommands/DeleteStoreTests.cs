using Application.Commands.StoreCommands.DeleteStore;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.StoreCommands
{
    public class DeleteStoreByIdCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<Store>> _mockStoreRepo;
        private readonly Mock<ILogger<DeleteStoreByIdCommandHandler>> _mockLogger;
        private readonly DeleteStoreByIdCommandHandler _handler;

        public DeleteStoreByIdCommandHandlerTests()
        {
            _mockStoreRepo = new Mock<IGenericRepository<Store>>();
            _mockLogger = new Mock<ILogger<DeleteStoreByIdCommandHandler>>();
            _handler = new DeleteStoreByIdCommandHandler(_mockStoreRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStoreNotFound()
        {
            var command = new DeleteStoreByIdCommand(1);

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Store)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Store not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStoreDeleted()
        {
            var command = new DeleteStoreByIdCommand(1);
            var store = new Store { Id = 1, Location = "Miami" };

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(store);

            _mockStoreRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Store deleted successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new DeleteStoreByIdCommand(1);

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while deleting store: Database error", result.ErrorMessage);
        }
    }
}
