﻿using Application.Helpers;
using Application.Interfaces;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.UserQueries.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<User> database;
        private readonly ITokenHelper tokenHelper;
        private readonly ILogger logger;
        public LoginUserCommandHandler(IGenericRepository<User> _database, ITokenHelper _tokenHelper, ILogger<LoginUserCommandHandler> _logger)
        {
            database = _database;
            tokenHelper = _tokenHelper;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var UserName = request.UserLogin.UserName;
            var UserPass = request.UserLogin.UserPass;
            try
            {
                var user = await database.GetFirstOrDefaultAsync(u => u.UserName == UserName, cancellationToken);
                if (user == null)
                {
                    return OperationResult<string>.FailureResult("User not found", logger);
                }
                if (!BCrypt.Net.BCrypt.Verify(UserPass, user.UserPass))
                {
                    return OperationResult<string>.FailureResult("Invalid password", logger);
                }
                return OperationResult<string>.SuccessResult(tokenHelper.GenerateToken(user), logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while checking user: {exception.Message}", logger);
            }
        }
    }
}
