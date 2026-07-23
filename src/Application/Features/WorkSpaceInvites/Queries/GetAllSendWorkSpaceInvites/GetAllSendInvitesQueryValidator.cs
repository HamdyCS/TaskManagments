using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetAllSendWorkSpaceInvites
{
    public class GetAllSendInvitesQueryValidator : AbstractValidator<GetAllSendInvitesQuery>
    {
        public GetAllSendInvitesQueryValidator()
        {
            RuleFor(x => x.InviteById).NotEmpty().WithMessage("InviteById is required.");
        }
    }
}
