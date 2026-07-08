using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandlerValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandHandlerValidator()
        {
            RuleFor(x=>x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x=>x.ResetPasswordDto.Token)
                .NotEmpty().WithMessage("Token is required");

            RuleFor(x => x.ResetPasswordDto.NewPassword)
               .NotEmpty().WithMessage("NewPassword is required")
               .MinimumLength(8).WithMessage("NewPassword must be at least 8 characters long")
               .MaximumLength(80).WithMessage("NewPassword must be at most 80 characters long")
               .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=-]).*$").
               WithMessage("NewPassword must contain at least one lowercase letter, one uppercase letter, one number and one special character");
        }
    }
}
