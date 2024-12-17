using Application.DTOs;
using Application.Helpers;
using MediatR;

namespace Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<OperationResult<string>>
    {
        public UserDTO UserLogin { get; set; }
        public LoginUserCommand(UserDTO userToLogin)
        {
            UserLogin = userToLogin;
        }
    }
}
