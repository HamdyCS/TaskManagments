using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.UpdateWorkSpace
{
    public class UpdateWorkSpaceCommandValidator : AbstractValidator<UpdateWorkSpaceCommand>
    {
        public UpdateWorkSpaceCommandValidator()
        {
            RuleFor(x=>x.UpdateWorkSpaceDto.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.UpdateBy)
                .NotEmpty().WithMessage("UpdateBy is required");

            RuleFor(x=>x.WorkSpaceId)
                .NotEmpty().WithMessage("WorkSpaceId is required");
        }
    }
}
