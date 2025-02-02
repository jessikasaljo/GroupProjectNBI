
using Application.Commands.StoreItemCommands.UpdateStoreItem;
using Application.DTOs.StoreItemDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.StoreItemCommands
{
    public class UpdateStoreItemTests
    {
        private readonly Mock<IGenericRepository<StoreItem>> _mockStoreItemRepo;
        private readonly Mock<ILogger<UpdateStoreItemCommandHandler>> _mockLogger;
        private readonly UpdateStoreItemCommandHandler _handler;

        public UpdateStoreItemTests()
        {
            _mockStoreItemRepo = new Mock<IGenericRepository<StoreItem>>();
            _mockLogger = new Mock<ILogger<UpdateStoreItemCommandHandler>>();
            _handler = new UpdateStoreItemCommandHandler(_mockStoreItemRepo.Object, _mockLogger.Object, null!);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStoreItemNotFound()
        {
            var command = new UpdateStoreItemCommand(new UpdateStoreItemDTO { Quantity = 10 }, 1);

            _mockStoreItemRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StoreItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((StoreItem)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("StoreItem not found", result.ErrorMessage);
        }
    }
}
