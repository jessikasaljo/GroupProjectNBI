
using Application.Commands.StoreItemCommands.DeleteStoreItem;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.StoreItemCommands
{
    public class DeleteStoreItemTests
    {
        private readonly Mock<IGenericRepository<StoreItem>> _mockStoreItemRepo;
        private readonly Mock<ILogger<DeleteStoreItemCommandHandler>> _mockLogger;
        private readonly DeleteStoreItemCommandHandler _handler;

        public DeleteStoreItemTests()
        {
            _mockStoreItemRepo = new Mock<IGenericRepository<StoreItem>>();
            _mockLogger = new Mock<ILogger<DeleteStoreItemCommandHandler>>();
            _handler = new DeleteStoreItemCommandHandler(_mockStoreItemRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStoreItemNotFound()
        {
            var command = new DeleteStoreItemByIdCommand(1);

            _mockStoreItemRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StoreItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((StoreItem)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("StoreItem not found", result.ErrorMessage);
        }
    }
}
