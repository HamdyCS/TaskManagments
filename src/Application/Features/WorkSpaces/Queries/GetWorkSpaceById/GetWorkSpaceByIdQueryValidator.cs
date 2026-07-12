using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetWorkSpaceById
{
    public class GetWorkSpaceByIdQueryValidator : AbstractValidator<GetWorkSpaceByIdQuery>
    {
        public GetWorkSpaceByIdQueryValidator()
        {
           RuleFor(x=>x.WorkSpaceId)
                .NotEmpty().WithMessage("WorkSpaceId is required");
        }
    }
}
