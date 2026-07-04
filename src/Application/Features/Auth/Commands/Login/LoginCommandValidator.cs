using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.loginDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is invalid");

            RuleFor(x => x.loginDto.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
