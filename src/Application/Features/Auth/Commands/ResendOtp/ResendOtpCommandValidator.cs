using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResendOtp
{
    public class ResendOtpCommandValidator : AbstractValidator<ResendOtpCommand>
    {
        public ResendOtpCommandValidator()
        {
            RuleFor(x=>x.ResendOtpDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x=>x.OtpPurpose)
                .NotNull().WithMessage("OtpPurpose is required");
        }
    }
}
