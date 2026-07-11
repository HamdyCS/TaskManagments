using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.CreateWorkSpace
{
    public class CreateWorkSpaceCommandValidator : AbstractValidator<CreateWorkSpaceCommand>
    {
        public CreateWorkSpaceCommandValidator()
        {
            RuleFor(x=>x.CreateWorkSpaceDto.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.CreateWorkSpaceDto.Description)
               .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.CreateBy)
                .NotEmpty().WithMessage("CreateBy is required");
        }
    }
}
