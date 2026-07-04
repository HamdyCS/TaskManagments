using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateRefreshToken
{
    public class CreateRefreshTokenCommandValidator : AbstractValidator<CreateRefreshTokenCommand>
    {
        public CreateRefreshTokenCommandValidator()
        {
            RuleFor(x => x.userId).NotEmpty().WithMessage("userId is required");
        }
    }
}
