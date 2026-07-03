using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.RegisterNewUser
{
    public class ConfirmationEmailCommandValidator : AbstractValidator<ConfirmationEmailCommand>
    {
        public ConfirmationEmailCommandValidator()
        {
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("email is required")
                .EmailAddress().WithMessage("email is not valid");

            RuleFor(x => x.token)
                .NotEmpty().WithMessage("token is required");

        }
    }
}
