
using Application.Commands.StoreItemCommands.AddStoreItem;
using Application.Commands.StoreItemCommands.DeleteStoreItem;
using Application.DTOs.StoreItemDtos;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.CommandTests.StoreItemCommands
{
    public class AddStoreItemTests
    {
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;
        private readonly Mock<IGenericRepository<Store>> _mockStoreRepo;
        private readonly Mock<ILogger<AddStoreItemCommandHandler>> _mockLogger;
        private readonly AddStoreItemCommandHandler _handler;

        public AddStoreItemTests()
        {
            _mockProductRepo = new Mock<IGenericRepository<Product>>();
            _mockStoreRepo = new Mock<IGenericRepository<Store>>();
            _mockLogger = new Mock<ILogger<AddStoreItemCommandHandler>>();
            _handler = new AddStoreItemCommandHandler(_mockProductRepo.Object, _mockStoreRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
        {
            var command = new AddStoreItemCommand(new AddStoreItemDTO { StoreId = 1, ProductId = 1, Quantity = 5 });

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Product with ID", result.ErrorMessage);
        }
    }
}
