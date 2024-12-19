using Application.DTOs;
using Application.Helpers;
using MediatR;

namespace Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<OperationResult<string>>
    {
        public LoginUserDTO UserLogin { get; set; }
        public LoginUserCommand(LoginUserDTO userToLogin)
        {
            UserLogin = userToLogin;
        }
    }
}
