using Application.DTOs.User;
using Application.Helpers;
using MediatR;

namespace Application.Queries.UserQueries.LoginUser
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
