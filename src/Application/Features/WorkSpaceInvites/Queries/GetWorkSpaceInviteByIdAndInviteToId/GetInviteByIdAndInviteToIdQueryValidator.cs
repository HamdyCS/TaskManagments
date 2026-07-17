using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetInviteByIdAndInviteToId
{
    public class GetInviteByIdAndInviteToIdQueryValidator : AbstractValidator<GetInviteByIdAndInviteToIdQuery>
    {
        public GetInviteByIdAndInviteToIdQueryValidator()
        {
            RuleFor(x => x.WorkSpaceInviteId)
                .NotNull().WithMessage("WorkSpaceInviteId is required.")
                .Must(x => x > 0).WithMessage("WorkSpaceInviteId must be greater than 0");

            RuleFor(x => x.InviteToId).NotEmpty().WithMessage("InviteToId is required.");
        }
    }
}
