using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.LoginByProvider
{
    public class LoginByProviderCommandValidator : AbstractValidator<LoginByProviderCommand>
    {
        public LoginByProviderCommandValidator()
        {
            RuleFor(x=>x.RoleOnCreate)
                .NotEmpty().WithMessage("RoleOnCreate is required");
        }
    }
}
