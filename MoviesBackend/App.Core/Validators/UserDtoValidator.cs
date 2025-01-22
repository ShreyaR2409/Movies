using App.Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(25).WithMessage("First Name should not exceed 25 character")
                .MinimumLength(2).WithMessage("Last Name should contain more than 2 character");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name cannot be empty")
                .MaximumLength(25).WithMessage("Last Name should not exceed 25 character")
                .MinimumLength(2).WithMessage("Last Name should contain more than 2 character");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .MaximumLength(35).WithMessage("Email should not be exceed 35 character")
                .EmailAddress();

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth cannot be null");
                
                
        }
    }
}
