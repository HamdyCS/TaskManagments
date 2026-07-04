using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x=>x.refreshToken)
                .NotEmpty().WithMessage("Refresh token is required");

            RuleFor(x=>x.userId)
                .NotEmpty().WithMessage("User Id is required");
        }
    }
}
