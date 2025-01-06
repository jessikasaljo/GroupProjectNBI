using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Commands.UserCommands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.newUser.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.newUser.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters")
                .Matches("^[a-zA-Z]+$").WithMessage("First name can only contain letters");

            RuleFor(x => x.newUser.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name can only contain letters");

            RuleFor(x => x.newUser.Phone).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x.newUser.Address).NotEmpty().WithMessage("Address is required");
        }
    }
}