using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.RegisterNewUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.registerNewUserDto)
                .NotEmpty().WithMessage("registerNewUserDto is required");

            RuleFor(x => x.registerNewUserDto.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MinimumLength(2).WithMessage("FirstName must be at least 2 characters long")
                .MaximumLength(50).WithMessage("FirstName must be at most 50 characters long");

            RuleFor(x => x.registerNewUserDto.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MinimumLength(2).WithMessage("LastName must be at least 2 characters long")
                .MaximumLength(50).WithMessage("LastName must be at most 50 characters long");

            RuleFor(x => x.registerNewUserDto.DateOfBirth)
                .NotNull().WithMessage("DateOfBirth is required")
                .Must(
                date => date.ToDateTime(TimeOnly.MinValue) <= DateTime.Today.AddYears(-18))
                .WithMessage("User must be at least 18 years old");


            RuleFor(x => x.registerNewUserDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");


            RuleFor(x => x.registerNewUserDto.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(80).WithMessage("Password must be at most 80 characters long")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=-]).*$").
                WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one number and one special character");

        }
    }
}
