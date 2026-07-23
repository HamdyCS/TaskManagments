using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetWorkSpaceInviteById
{
    public class GetInviteByIdQueryValidator : AbstractValidator<GetInviteByIdQuery>
    {
        public GetInviteByIdQueryValidator()
        {
            RuleFor(x => x.WorkSpaceInviteId)
                .NotNull().WithMessage("WorkSpaceInviteId is required.")
                .Must(x => x > 0).WithMessage("WorkSpaceInviteId must be greater than 0");

        }
    }
}
