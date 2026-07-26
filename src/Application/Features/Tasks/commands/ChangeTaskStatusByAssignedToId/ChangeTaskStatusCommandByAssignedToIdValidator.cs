using Domain.Common.Enums;
using FluentValidation;

namespace Application.Features.Tasks.Commands.ChangeTaskStatusByAssignedToId
{
    public class ChangeTaskStatusCommandByAssignedToIdValidator : AbstractValidator<ChangeTaskStatusCommandByAssignedToId>
    {
        public ChangeTaskStatusCommandByAssignedToIdValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ChangeTaskStatusDto.Status)
                .IsInEnum().WithMessage("Invalid task status");

            RuleFor(x=>x.AssignedToId)
                .NotEmpty().WithMessage("AssignedToId is required");

            RuleFor(x=>x.ProjectId)
                .GreaterThan(0).WithMessage("ProjectId must be greater than 0");

            RuleFor(x => x.TaskId)
               .GreaterThan(0).WithMessage("TaskId must be greater than 0");

        }    
    }
}
