using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
    {
        public VerifyOtpCommandValidator()
        {
            RuleFor(x=>x.VerifyOtpDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x=>x.VerifyOtpDto.Otp)
                .NotEmpty().WithMessage("Otp is required");

            RuleFor(x=>x.OtpPurpose)
                .NotNull().WithMessage("OtpPurpose is required");
        }
    }
}
