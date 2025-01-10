using Application.DTOs.User;
using Application.Helpers;
using MediatR;

namespace Application.Commands.UserCommands.AddUser
{
    public class AddUserCommand : IRequest<OperationResult<string>>
    {
        public AddUserDTO newUser { get; set; }
        public AddUserCommand(AddUserDTO userToAdd)
        {
            newUser = userToAdd;
        }
    }
}