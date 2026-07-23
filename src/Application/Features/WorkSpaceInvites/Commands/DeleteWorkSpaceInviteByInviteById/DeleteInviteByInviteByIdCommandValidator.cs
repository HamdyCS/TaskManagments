using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.DeleteWorkSpaceInviteByInviteById
{
    public class DeleteInviteByInviteByIdCommandValidator : AbstractValidator<DeleteInviteByInviteByIdCommand>
    {
        public DeleteInviteByInviteByIdCommandValidator()
        {
            RuleFor(x => x.WorkSpaceInviteId)
                .NotNull().WithMessage("WorkSpaceInviteId is required")
                .Must(x => x > 0).WithMessage("WorkSpaceInviteId must be greater than 0");

            RuleFor(x => x.InviteById)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
