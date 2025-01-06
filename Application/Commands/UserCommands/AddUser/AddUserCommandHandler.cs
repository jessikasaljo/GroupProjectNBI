using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<User> Database;
        private readonly ILogger<AddUserCommandHandler> logger;
        public AddUserCommandHandler(IGenericRepository<User> _Database, ILogger<AddUserCommandHandler> _logger)
        {
            Database = _Database;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            User userToCreate = new()
            {
                UserName = request.newUser.UserName,
                UserPass = BCrypt.Net.BCrypt.HashPassword(request.newUser.UserPass),
                Email = request.newUser.Email,
                FirstName = request.newUser.FirstName,
                LastName = request.newUser.LastName,
                Phone = request.newUser.Phone,
                Address = request.newUser.Address,
                Role = 0
            };
            User? existingUser = null;
            try
            {
                existingUser = await Database.GetFirstOrDefaultAsync(u => u.UserName == userToCreate.UserName, cancellationToken);
                if (existingUser != null)
                {
                    return OperationResult<string>.FailureResult("User already exists", logger);
                }

                await Database.AddAsync(userToCreate, cancellationToken);
                return OperationResult<string>.SuccessResult("User added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking user: {exception.Message}", logger);
            }
        }
    }
}
