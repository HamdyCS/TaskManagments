using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateExternalAuthProperty
{
    public class CreateExternalAuthPropertyCommandValidator : AbstractValidator<CreateExternalAuthPropertyCommand>
    {
        public CreateExternalAuthPropertyCommandValidator(IConfiguration configuration)
        {
          

            RuleFor(x => x.Provider)
                .NotEmpty().WithMessage("Provider is required");

            RuleFor(x => x.redirectUrl)
                .NotEmpty().WithMessage("redirectUrl is required")
                .Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute))
                .WithMessage("redirectUrl is not valid");
        }
    }
}
