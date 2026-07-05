using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetUserById
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x=>x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.UpdateUserDto.FirstName)
                .NotEmpty().WithMessage("FirstName is required");

            RuleFor(x => x.UpdateUserDto.LastName)
                .NotEmpty().WithMessage("LastName is required");

            RuleFor(x=>x.UpdateUserDto.DateOfBirth)
                .NotNull().WithMessage("DateOfBirth is required")
                .Must(x=>x.ToDateTime(TimeOnly.MinValue) 
                <= DateTime.UtcNow.AddYears(-18)).WithMessage("User must be at least 18 years old");
        }
    }
}
