using Application.Commands.UserCommands.AddUser;
using Application.DTOs.User;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.CommandTests.UserCommands
{
    public class AddUserTests
    {
        private readonly Mock<IGenericRepository<User>> _mockUserRepo;
        private readonly Mock<ILogger<AddUserCommandHandler>> _mockLogger;
        private readonly AddUserCommandHandler _handler;

        public AddUserTests()
        {
            _mockUserRepo = new Mock<IGenericRepository<User>>();
            _mockLogger = new Mock<ILogger<AddUserCommandHandler>>();

            _handler = new AddUserCommandHandler(
                _mockUserRepo.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserAlreadyExists()
        {
            var command = new AddUserCommand(new AddUserDTO { UserName = "existingUser ", UserPass = "1234", FirstName = "Jane", LastName = "Doe" });
            var existingUser = new User { UserName = "existingUser ", UserPass = "1234", FirstName = "Jane", LastName = "Doe" };

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("User already exists", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUserIsAdded()
        {
            var command = new AddUserCommand(new AddUserDTO { UserName = "newUser ", UserPass = "1234", FirstName = "Jane", LastName = "Doe" });
            var newUser = new User { UserName = "newUser ", UserPass = "1234", FirstName = "Jane", LastName = "Doe" };

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null!);
            _mockUserRepo.Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("User added successfully", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new AddUserCommand(new AddUserDTO { UserName = "newUser ", UserPass = "1234", FirstName = "Jane", LastName = "Doe" });

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Contains("Error occurred while checking user: Database error", result.ErrorMessage);
        }
    }
}