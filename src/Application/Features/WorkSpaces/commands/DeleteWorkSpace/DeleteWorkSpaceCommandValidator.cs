using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.DeleteWorkSpace
{
    public class DeleteWorkSpaceCommandValidator : AbstractValidator<DeleteWorkSpaceCommand>
    {
        public DeleteWorkSpaceCommandValidator()
        {

            RuleFor(x => x.DeleteBy)
                .NotEmpty().WithMessage("DeleteBy is required");

            RuleFor(x=>x.WorkSpaceId)
                .NotEmpty().WithMessage("WorkSpaceId is required");
        }
    }
}
