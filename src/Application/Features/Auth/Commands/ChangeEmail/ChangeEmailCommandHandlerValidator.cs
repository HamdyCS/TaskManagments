using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ChangeEmail
{
    public class ChangeEmailCommandHandlerValidator : AbstractValidator<ChangeEmailCommand>
    {
        public ChangeEmailCommandHandlerValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.ChangeEmailDto.Token)
                .NotEmpty().WithMessage("Token is required");

            RuleFor(x => x.ChangeEmailDto.NewEmail)
               .NotEmpty().WithMessage("NewEmail is required")
               .EmailAddress().WithMessage("NewEmail is not valid");
        }
    }
}
