using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.CreateWorkSpaceInvite
{
    public class CreateInviteCommandValidator : AbstractValidator<CreateInviteCommand>
    {
        public CreateInviteCommandValidator()
        {
            RuleFor(x=>x.CreateWorkSpaceInviteDto.InviteToEmail)
                .NotEmpty().WithMessage("InviteToEmail is required")
                .EmailAddress().WithMessage("InviteToEmail is not valid");

            RuleFor(x => x.CreateWorkSpaceInviteDto.WorkSpaceId)
                .NotNull().WithMessage("WorkSpaceId is required")
                .Must(x => x > 0).WithMessage("WorkSpaceId must be greater than 0");

            RuleFor(x => x.CreateWorkSpaceInviteDto.WorkSpaceRole)
               .NotNull().WithMessage("WorkSpaceRole is required");
           

            RuleFor(x=>x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
