using Application.DTOs;
using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.AddUser
{
    public class AddUserCommand : IRequest<OperationResult<string>>
    {
        public UserDTO newUser { get; set; }
        public AddUserCommand(UserDTO userToAdd)
        {
            newUser = userToAdd;
        }
    }
}