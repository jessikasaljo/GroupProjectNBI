using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FluentValidation;

namespace Application.Commands.UserCommands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<AddUserCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly AddUserCommandValidator _validator;

        public AddUserCommandHandler(IGenericRepository<User> userRepository, ILogger<AddUserCommandHandler> logger, IMapper mapper, AddUserCommandValidator validator)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<OperationResult<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return OperationResult<string>.FailureResult(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), _logger);
            }

            var userToCreate = _mapper.Map<User>(request.newUser);

            var existingUser = await _userRepository.GetFirstOrDefaultAsync(u => u.UserName == userToCreate.UserName, cancellationToken);
            if (existingUser != null)
            {
                return OperationResult<string>.FailureResult("User already exists", _logger);
            }

            await _userRepository.AddAsync(userToCreate, cancellationToken);
            return OperationResult<string>.SuccessResult("User added successfully", _logger);
        }
    }
}