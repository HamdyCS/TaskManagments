using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetAllUserWorkSpaces
{
    public class GetAllUserWorkSpacesQueryValidator : AbstractValidator<GetAllUserWorkSpacesQuery>
    {
        public GetAllUserWorkSpacesQueryValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("User Id is required");

        }
    }
}
