using Application.DTOs.User;
using Application.Interfaces;
using Application.Queries.UserQueries.LoginUser;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests.UnitTests.QueryTests.UserQueries
{
    public class LoginUserTests
    {
        private readonly Mock<IGenericRepository<User>> _mockUserRepo;
        private readonly Mock<ILogger<LoginUserCommandHandler>> _mockLogger;
        private readonly Mock<ITokenHelper> _mockTokenHelper;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserTests()
        {
            _mockUserRepo = new Mock<IGenericRepository<User>>();
            _mockLogger = new Mock<ILogger<LoginUserCommandHandler>>();
            _mockTokenHelper = new Mock<ITokenHelper>();

            _handler = new LoginUserCommandHandler(
                _mockUserRepo.Object,
                _mockTokenHelper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            var command = new LoginUserCommand(new LoginUserDTO { UserName = "nonexistentUser", UserPass = "password" });

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("User not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenInvalidPassword()
        {
            var correctPassword = "correctPassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            var command = new LoginUserCommand(new LoginUserDTO { UserName = "userToLogin", UserPass = "wrongPassword" });

            var existingUser = new User
            {
                UserName = "userToLogin",
                UserPass = hashedPassword,
                FirstName = "Jane",
                LastName = "Doe"
            };

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Invalid password", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUserLogsIn()
        {
            var correctPassword = "1234";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            var command = new LoginUserCommand(new LoginUserDTO { UserName = "existingUser", UserPass = correctPassword });
            var existingUser = new User { UserName = "existingUser", UserPass = hashedPassword, FirstName = "Jane", LastName = "Doe" };

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);
            _mockTokenHelper.Setup(th => th.GenerateToken(It.IsAny<User>())).Returns("token");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("token", result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var command = new LoginUserCommand(new LoginUserDTO { UserName = "existingUser", UserPass = "password" });

            _mockUserRepo.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("Error occurred while checking user: Database error", result.ErrorMessage);
        }
    }
}
