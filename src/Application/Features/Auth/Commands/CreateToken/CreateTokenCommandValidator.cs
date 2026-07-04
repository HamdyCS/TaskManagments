using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateToken
{
    public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenCommandValidator()
        {
            RuleFor(x=>x.refreshToken)
                .NotEmpty().WithMessage("Refresh token cannot be empty");

            RuleFor(x=>x.userId)
                .NotEmpty().WithMessage("User Id cannot be empty");

        }
    }
}
