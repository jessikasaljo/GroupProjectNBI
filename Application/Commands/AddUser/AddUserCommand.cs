using Application.DTOs;
using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.AddUser
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