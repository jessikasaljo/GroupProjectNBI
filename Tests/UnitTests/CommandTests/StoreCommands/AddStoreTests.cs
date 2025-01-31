using Application.Commands.StoreCommands.AddStore;
using Application.DTOs.StoreDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.StoreCommands
{
    public class AddStoreCommandHandlerTests
    {
        private readonly Mock<IGenericRepository<Store>> _mockStoreRepo;
        private readonly Mock<ILogger<AddStoreCommandHandler>> _mockLogger;
        private readonly AddStoreCommandHandler _handler;

        public AddStoreCommandHandlerTests()
        {
            _mockStoreRepo = new Mock<IGenericRepository<Store>>();
            _mockLogger = new Mock<ILogger<AddStoreCommandHandler>>();
            _handler = new AddStoreCommandHandler(_mockStoreRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStoreAlreadyExists()
        {
            var command = new AddStoreCommand(new AddStoreDTO { Location = "New York" });

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Store { Location = "New York" });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Store already exists at this location", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStoreAddedSuccessfully()
        {
            var command = new AddStoreCommand(new AddStoreDTO { Location = "Los Angeles" });

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Store)null!);

            _mockStoreRepo.Setup(repo => repo.AddAsync(It.IsAny<Store>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Store added successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new AddStoreCommand(new AddStoreDTO { Location = "Miami" });

            _mockStoreRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Store, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while adding store: Database error", result.ErrorMessage);
        }
    }
}
