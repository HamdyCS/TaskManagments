using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.UpdateWorkSpaceInviteStatus
{
    public class UpdateInviteStatusCommandValidator : AbstractValidator<UpdateInviteStatusCommand>
    {
        public UpdateInviteStatusCommandValidator()
        {
            RuleFor(x => x.InviteToId)
                .NotEmpty().WithMessage("InviteToEmail is required");

            RuleFor(x => x.WorkSpaceInviteId)
               .NotNull().WithMessage("WorkSpaceInviteId is required")
               .Must(x => x > 0).WithMessage("WorkSpaceInviteId must be greater than 0");

            RuleFor(x => x.WorkSpaceInviteStatus)
              .NotNull().WithMessage("WorkSpaceInviteStatus is required");
           
        }
    }
}
