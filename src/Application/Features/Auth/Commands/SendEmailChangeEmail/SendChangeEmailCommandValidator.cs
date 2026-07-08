using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendEmailChangeEmail
{
    public class SendChangeEmailCommandValidator : AbstractValidator<SendChangeEmailCommand>
    {
        public SendChangeEmailCommandValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("NewEmail is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
