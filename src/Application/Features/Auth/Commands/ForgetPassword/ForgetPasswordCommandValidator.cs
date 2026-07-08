using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x=>x.ForgetPasswordDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x=>x.ForgetPasswordDto.Otp)
                .NotEmpty().WithMessage("Otp is required");


            RuleFor(x => x.ForgetPasswordDto.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(80).WithMessage("Password must be at most 80 characters long")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=-]).*$").
                WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one number and one special character");

        }
    }
}
