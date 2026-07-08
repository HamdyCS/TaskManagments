using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendPasswordResetEmail
{
    public class SendPasswordResetEmailCommandValidator : AbstractValidator<SendPasswordResetEmailCommand>
    {
        public SendPasswordResetEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
