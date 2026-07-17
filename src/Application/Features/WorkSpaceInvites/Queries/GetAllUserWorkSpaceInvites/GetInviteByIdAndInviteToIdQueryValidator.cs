using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetAllUserInvites
{
    public class GetInviteByIdAndInviteToIdQueryValidator : AbstractValidator<GetInviteByIdAndInviteToIdQuery>
    {
        public GetInviteByIdAndInviteToIdQueryValidator()
        {
            RuleFor(x => x.InviteToId).NotEmpty().WithMessage("InviteToId is required.");
        }
    }
}
